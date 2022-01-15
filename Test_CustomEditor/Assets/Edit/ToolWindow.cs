using System.Collections;
using System.Collections.Generic;

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

    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    void OnSceneGUI(SceneView obj)
    {
        var mousePos = Event.current.mousePosition;
        var ray = HandleUtility.GUIPointToWorldRay(mousePos);

        EditorHelper.RayCast(ray.origin, ray.origin + ray.direction * 300, out var hitPos);
        //Debug.Log(FindObjectOfType<CustomGrid>().GetCellPos(hitPos  ));
    }
    private void OnGUI()
    {
        if (currentMode == EditMode.Create)
        {
            DrawCreateMode();

        }
        else if(currentMode == EditMode.Edit)
        {
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
            targetGrid = BuildGrid(cellCount, cellSize);
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

            }
            if (GUILayout.Button("저장하기", EditorStyles.toolbarButton))
            {

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
}
