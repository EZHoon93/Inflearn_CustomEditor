using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEditor;

using UnityEngine;

public enum EditMode
{
    None = 0,
    Create,
    Edit
}

public enum EditToolMode
{
    Paint,
    Erase
}

public class ToolWindow : EditorWindow
{
    public EditMode currentMode = EditMode.None;

    public EditToolMode selectedEditToolMode = EditToolMode.Paint;
    public GUIContent[] editToolModeContents;
    public CustomGridPalette targetPaletee;
    public CustomGridPaletteDrawer plaetteDrawer = new CustomGridPaletteDrawer();
    public Vector2Int cellCount;
    public Vector2 cellSize;

    public CustomGrid targetGrid;
    bool isCreateble => cellCount.x > 0 && cellCount.y > 0 && cellSize.x >  0 && cellSize.y > 0  && targetPaletee != null;

    [MenuItem("My Tool/Open My Tool &g")]
    static void Open()
    {
        var window = GetWindow<ToolWindow>();
        window.title = "this is my ToolWindow";
    }
    private void OnEnable()
    {
        editToolModeContents = new GUIContent[]
        {
            EditorGUIUtility.TrIconContent("Grid.PaintTool", "그리기모드"),       //아이콘  URL참조 github.com/halak/unity-editor-icons
            EditorGUIUtility.TrIconContent("Grid.EraserTool", "지우기 모드"),

        };

        ChangeMode(EditMode.Create);
        SceneView.duringSceneGui -= OnSceneGUI;
        SceneView.duringSceneGui += OnSceneGUI;

        Undo.undoRedoPerformed += OnUndoRedoPerformed;
    }

    void OnUndoRedoPerformed()
    {

    }

    private void OnDisable()
    {
        Clear();
        SceneView.duringSceneGui -= OnSceneGUI;
        Undo.undoRedoPerformed -= OnUndoRedoPerformed;

    }
    private void Update()
    {
        SceneView.lastActiveSceneView.Repaint();
    }
    void OnSceneGUI(SceneView obj)
    {

        if(currentMode  != EditMode.Edit)
        {
            return;
        }
        var mousePos = Event.current.mousePosition;
        var ray = HandleUtility.GUIPointToWorldRay(mousePos);
        EditorHelper.RayCast(ray.origin, ray.origin + ray.direction * 300, out var hitPos);
        var cellPos = targetGrid.GetCellPos(hitPos);


        if (Event.current.button == 0 && Event.current.type == EventType.MouseDown)
        {
       
            //마우스가 그리드내에잇을때
            if (targetGrid.Contains(cellPos))
            {
                if(selectedEditToolMode == EditToolMode.Paint)
                {
                    Paint(cellPos);
                }
                else if(selectedEditToolMode == EditToolMode.Erase)
                {
                    Erase(cellPos);
                }
            }
        }

        Handles.BeginGUI();
        {
            GUI.Label(new Rect(mousePos.x, mousePos.y + 10, 100, 50), cellPos.ToString(), EditorStyles.boldLabel);
            if (targetGrid.IsItemExist(cellPos))
            {
                var item = targetPaletee.GetItem(targetGrid.GetItem(cellPos).id);
                var previewTex = AssetPreview.GetAssetPreview(item.targetObject);

                var rtBox = new Rect(10, 10, previewTex.width + 10, previewTex.height + 10);
                var reTex = new Rect(15, 15, previewTex.width, previewTex.height);

                GUI.Box(rtBox, GUIContent.none, GUI.skin.window);
                GUI.DrawTexture(reTex, previewTex);

                var rtName = new Rect(rtBox.center.x - 10, rtBox.bottom - 25, 100, 10);
                GUI.Label(rtName, item.name, EditorStyles.boldLabel);
            }
        }
        Handles.EndGUI();
        //Debug.Log(FindObjectOfType<CustomGrid>().GetCellPos(hitPos  ));
    }

    void Paint(Vector2Int cellPos)
    {
        var selectedItem = plaetteDrawer.SelectedItem;
        if(selectedItem == null)
        {
            return;
        }
        if (targetGrid.IsItemExist(cellPos))
        {
            //DestroyImmediate(targetGrid.GetItem(cellPos).gameObject);
            Undo.DestroyObjectImmediate(targetGrid.GetItem(cellPos).gameObject);
            targetGrid.RemoveItem(cellPos);
        }

        var target = targetGrid.AddItem(cellPos, selectedItem);

        Undo.RegisterCompleteObjectUndo(target.gameObject, "Create MapObject!");

        Event.current.Use();        //이벤트있는것 소거..
    }

    void Erase(Vector2Int cellPos)
    {
        if (targetGrid.IsItemExist(cellPos))
        {
            //DestroyImmediate(targetGrid.GetItem(cellPos).gameObject);
            Undo.DestroyObjectImmediate(targetGrid.GetItem(cellPos).gameObject);
            targetGrid.RemoveItem(cellPos);
            Event.current.Use();

        }
    }
    private void OnGUI()
    {
        if (currentMode == EditMode.Create)
        {
            DrawCreateMode();

        }
        else if(currentMode == EditMode.Edit)
        {
            if(Event.current.keyCode == KeyCode.Q  && Event.current.type == EventType.KeyDown)
            {
                this.selectedEditToolMode = EditToolMode.Paint;
                Repaint();
                Event.current.Use();
            }
            else if (Event.current.keyCode == KeyCode.W && Event.current.type == EventType.KeyDown)
            {
                this.selectedEditToolMode = EditToolMode.Erase;
                Repaint();
                Event.current.Use();
            }
            DrawEditMode();
        }
    }
    void DrawCreateMode()
    {
        EditorHelper.DrawCenterLabel(new GUIContent("생성모드"), Color.green,20, FontStyle.BoldAndItalic);

        using (var scope = new GUILayout.VerticalScope(GUI.skin.window))
        {
            cellCount = EditorGUILayout.Vector2IntField("Cell 개수", cellCount);
            cellSize = EditorGUILayout.Vector2Field("Cell 개수", cellSize);

            targetPaletee = (CustomGridPalette) EditorGUILayout.ObjectField("연결 할 팔레트", targetPaletee, typeof(CustomGridPalette));  //팔레트를설정할 필드 생성
            plaetteDrawer.TargetPalette = targetPaletee;
        }

        GUI.enabled = isCreateble;
        if (EditorHelper.DrawCenterButton("생성하기" , new Vector2(100,50)))
        {
            targetGrid = BuildGrid(this.cellCount, this.cellSize);
            ChangeMode(EditMode.Edit);
        }
        GUI.enabled = true;

    }

    private CustomGrid BuildGrid(Vector2Int cellCount , Vector2 cellSize)
    {
      
        Clear();
        var grid = new GameObject("Grid").AddComponent<CustomGrid>();
        grid.config = new CustomGridConfig();
        grid.config.CellCount = cellCount;
        grid.config.CellSize= cellSize;
        return grid;

    }
    void DrawEditMode()
    {
        GUILayout.BeginHorizontal(EditorStyles.toolbar);
        {
            if(GUILayout.Button("생성모드 ", EditorStyles.toolbarButton))
            {
                Clear();
                ChangeMode(EditMode.Edit);
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("불러오기", EditorStyles.toolbarButton))
            {
                Load();
            }
            if (GUILayout.Button("저장하기", EditorStyles.toolbarButton))
            {
                Save();
            }

        }

        GUILayout.EndHorizontal();

        EditorHelper.DrawCenterLabel(new GUIContent("편집모드"), Color.red, 20, FontStyle.BoldAndItalic);

        GUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            selectedEditToolMode = (EditToolMode)GUILayout.Toolbar((int)selectedEditToolMode, editToolModeContents, "LargeButton", GUILayout.Width(100), GUILayout.Height(40));
            GUILayout.FlexibleSpace();

        }
        GUILayout.EndHorizontal();

        var lastRect = GUILayoutUtility.GetLastRect(); //편집의 마지막부분 영역을 가져옴
        var area = new Rect(0, lastRect.yMax, position.width, position.height - lastRect.yMax - 1);     // 윈도우사이즈 - 버튼부터 영역을뺀 영역

        GUI.Box(area, GUIContent.none, GUI.skin.window);

        plaetteDrawer.Draw(new Vector2(position.width, position.height)); //에디터윈도우의 사이즈를 넘김
    }

    void ChangeMode(EditMode newMode)
    {
        if(currentMode == newMode)
        {
            return;
        }

        switch (newMode)
        {
            case EditMode.Create:

                break;
            case EditMode.Edit:
                SceneView.lastActiveSceneView.in2DMode = true;

                break;
            case EditMode.None:

                break;
        }

        currentMode = newMode;

    }

    void Clear()
    {
        var existings = FindObjectsOfType<CustomGrid>();

        if (existings != null)
        {
            for (int i = 0; i < existings.Length; i++)
            {
                DestroyImmediate(existings[i].gameObject);
            }
        }

        targetGrid = null;
    }

    void Save()
    {
        var path = EditorUtility.SaveFilePanel("맵 데이터 저장", Application.dataPath, "MapData.bin", "bin");
        if(string.IsNullOrEmpty(path) == false)
        {
            byte[] data = targetGrid.Serialize();

            File.WriteAllBytes(path , data);

            ShowNotification(new GUIContent("저장 성공") , 3.0f);
        }
    }

    void Load()
    {
        var path = EditorUtility.OpenFilePanel("맵 데이터 불러오기" , Application.dataPath ,"bin");
        if(string.IsNullOrEmpty(path) == false)
        {
            var data = File.ReadAllBytes(path);
            if(data != null)
            {
                targetGrid.Impot(data, targetPaletee);
            }
        }
    }
}
