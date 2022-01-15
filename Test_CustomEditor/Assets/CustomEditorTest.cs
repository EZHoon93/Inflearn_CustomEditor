using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CustomScript))]
public class CustomEditorTest : Editor
{
    CustomScript targetRef;

    SerializedProperty targetObjectProp;
    SerializedProperty nameProp;
    SerializedProperty hpProp;
  
    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
        targetObjectProp = serializedObject.FindProperty($"{nameof(CustomScript.otherObject)}");
        nameProp = serializedObject.FindProperty($"{nameof(CustomScript.myName)}");
        hpProp = serializedObject.FindProperty($"{nameof(CustomScript.myHP)}");

        //hpProp = serializedObject.FindProperty("myHP");

        targetRef = (CustomScript)base.target;

    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;

    }

    private void OnSceneGUI(SceneView obj)
    {
        //라벨을띄움
        Handles.Label(targetRef.transform.position, $"IAm {targetRef.gameObject.name}");

        var otherObjs = FindObjectsOfType<CustomScript>();

        for(int i = 0;  i < otherObjs.Length; i++)
        {
            if(this.targetRef != otherObjs[i])
            {
                var pos = otherObjs[i].transform.position;
                //선택된게임 오브젝트에서 다른게임오브젝트까지 라인그려줌
                Handles.DrawLine(this.targetRef.transform.position, pos);

                Handles.color = Color.red;
                Handles.DrawWireCube(pos, Vector3.one);
                Handles.color = Color.white;
            }
        }

        Handles.DrawWireCube(targetRef.transform.position, new Vector3(2, 3, 2)); //내가선택한 큐브를 그려줌

        Handles.BeginGUI();
        {
            if(GUILayout.Button("Move RRRR"))
            {
                targetRef.transform.position += Vector3.right;
            }
            if (GUILayout.Button("Move  LLL" +this.targetRef.name))
            {
                targetRef.transform.position -= Vector3.right;

            }
        }

        Handles.EndGUI();

    }

    public override void OnInspectorGUI()
    {
        //targetRef.myHP = EditorGUILayout.IntField(targetRef.myHP);
        serializedObject.Update();      //=> 어디선가 해당변수의 값이 바꼇을 지도모르니 일단 값들을 갖고온다.

        #region
        if (hpProp.intValue < 500)
        {
            GUI.color = Color.red;
        }
        else
        { 
            GUI.color = Color.green;
        }

        hpProp.intValue = EditorGUILayout.IntSlider("HP 값 : ", hpProp.intValue, 0, 1000);

        EditorGUILayout.BeginHorizontal();
        {
            GUI.color = Color.blue;
            EditorGUILayout.PrefixLabel("이름");
            GUI.color = Color.white;
            nameProp.stringValue = EditorGUILayout.TextArea(nameProp.stringValue);
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.PropertyField(targetObjectProp);

        serializedObject.ApplyModifiedProperties();
        #endregion

        #region 
        //EditorGUILayout.PropertyField(nameProp);
        //EditorGUILayout.PropertyField(hpProp);
        //EditorGUILayout.PropertyField(targetObjectProp);
        //serializedObject.ApplyModifiedProperties();
        #endregion

    }

}
