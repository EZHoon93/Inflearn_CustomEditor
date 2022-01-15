using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MyEditorWindow2 : EditorWindow
{
    Editor duplicatedEditor;
    Editor[] duplicatedDetailEditor;

    List<bool> detailFoldOut = new List<bool>();
    string stringVar;
    int intValue;



    [MenuItem("MyTool/Open MyToo2l %c")]
    static void Open()
    {
        var window = GetWindow<MyEditorWindow2>();
        window.title = "My Tool. EZ";
    }

    private void OnGUI()
    {
        if(Selection.objects != null && Selection.objects.Length == 1)
        {
            var target = Selection.objects[0];
            if(duplicatedEditor == null || duplicatedEditor.name != target.name)
            {
                duplicatedEditor = Editor.CreateEditor(target);

                var go = target as GameObject;
                if(go != null)
                {
                    var allComps = go.GetComponents<Component>();
                    if(allComps != null)
                    {
                        duplicatedDetailEditor = new Editor[allComps.Length];
                        for(int i = 0; i < allComps.Length;  i++)
                        {
                            detailFoldOut.Add(false);
                            duplicatedDetailEditor[i] = Editor.CreateEditor(allComps[i]);
                        }
                    }

                    else
                    {
                        detailFoldOut.Clear();
                        duplicatedEditor = null;
                    }
                }
            }
       
        }
        else
        {
            duplicatedEditor = null;
        }
        if ( duplicatedEditor != null)
        {
            duplicatedEditor.DrawHeader();
            duplicatedEditor.OnInspectorGUI();

            if(duplicatedDetailEditor != null)
            {
                for(int i = 0; i  < duplicatedDetailEditor.Length; i++)
                {
                    detailFoldOut[i] = EditorGUILayout.Foldout(detailFoldOut[i], $"{duplicatedDetailEditor[i].GetType()}");
                    if (detailFoldOut[i])
                    {
                        duplicatedDetailEditor[i].OnInspectorGUI();
                    }
                }
            }
        }
    }

}
    
