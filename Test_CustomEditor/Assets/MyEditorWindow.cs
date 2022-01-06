using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MyEditorWindow : EditorWindow
{

    Dictionary<SerializedObject, List<SerializedProperty>> Targets = new Dictionary<SerializedObject, List<SerializedProperty>>();

    bool _isFocused;

    [MenuItem("MyTool/Open MyTool %g")]
    static void Open()
    {
        var window = GetWindow<MyEditorWindow>();
        window.title = "My Tool. EZ";
    }
    private void Update()
    {
        if(!_isFocused)
        {
            foreach(var item in Targets)
            {
                item.Key.Update();
            }
        }

        Repaint();
    }

    private void OnGUI()
    {
        //버튼클릭시 다시 값을리셋후 가져옴
        if (GUILayout.Button("Refe"))
        {
            Targets.Clear();

            var allCustoms = FindObjectsOfType<CustomScript>();

            if (allCustoms != null)
            {
                for(int i = 0; i < allCustoms.Length; i++)
                {
                    var so = new SerializedObject(allCustoms[i]);
                    var props = new List<SerializedProperty>()
                    {
                        so.FindProperty(nameof(CustomScript.otherObject)),
                        so.FindProperty(nameof(CustomScript.myName)),
                        so.FindProperty(nameof(CustomScript.myHP)),
                    };
                    Targets.Add(so, props);
                }
            }

        }


        foreach(var pair in Targets)
        {
            EditorGUI.BeginChangeCheck();
            {
                EditorGUILayout.LabelField(pair.Key.targetObject.name, EditorStyles.boldLabel);
                EditorGUI.indentLevel++;    //자식처럼 보이는효과 , 들여쓰기
                {
                    foreach(var prop in pair.Value)
                    {
                        EditorGUILayout.PropertyField(prop);
                    }
                }
                EditorGUI.indentLevel--;    //복구

                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);  //선을 그음

                //먼가 값이 바뀐것이 감지되면 변수 업데이트적용.
                if (EditorGUI.EndChangeCheck())
                {
                    pair.Key.ApplyModifiedProperties(); //이것을해줘야 해당화면이아닌  이변수를 쓰는 다른곳에서도 값이변함.
                }

            }
        }
    }

    private void OnFocus()
    {
        _isFocused = true;
    }

    private void OnLostFocus()
    {
        _isFocused = false;

    }
}
