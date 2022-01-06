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

        targetObjectProp = serializedObject.FindProperty($"{nameof(CustomScript.otherObject)}");
        nameProp = serializedObject.FindProperty($"{nameof(CustomScript.myName)}");
        hpProp = serializedObject.FindProperty($"{nameof(CustomScript.myHP)}");

        //hpProp = serializedObject.FindProperty("myHP");

        targetRef = (CustomScript)base.target;

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
