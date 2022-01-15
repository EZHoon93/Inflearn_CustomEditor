using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MyEditorWindow3 : EditorWindow
{
    string stringVar;
    int intValue;
    [MenuItem("MyTool/Open MyToo3l %c")]
    static void Open()
    {
        var window = GetWindow<MyEditorWindow3>();
        window.title = "My Tool. E3Z";
    }

    private void OnGUI()
    {
        #region Secltion
        //if (GUILayout.Button("모든 씬오브젝트 선택"))
        //{
        //    var targets = FindObjectsOfType<GameObject>();
        //    if(targets != null)
        //    {
        //        Selection.objects = targets;
        //    }
        //}
        //if (GUILayout.Button("선택한 object핑  찍기"))
        //{
        //    if(Selection.objects != null  && Selection.objects.Length == 1)
        //    {
        //        EditorGUIUtility.PingObject(Selection.objects[0]);
        //    }
        //}
        #endregion

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.PrefixLabel("string");
            stringVar = GUILayout.TextArea(stringVar);
        }
        EditorGUILayout.EndHorizontal();
        intValue = EditorGUILayout.IntField("int", intValue);

        if (GUILayout.Button("값 저장"))
        {
            EditorPrefs.SetString("MyStringKey", stringVar);
            EditorPrefs.SetInt("MyStringInt", intValue  );

        }
        if (GUILayout.Button("값 로드"))
        {
            stringVar = EditorPrefs.GetString("MyStringKey");
            intValue = EditorPrefs.GetInt("MyStringInt");
        }
        
        if (GUILayout.Button("값 삭제"))
        {
             EditorPrefs.DeleteKey("MyStringKey");
             EditorPrefs.DeleteKey("MyStringInt");

        }
    }



}

