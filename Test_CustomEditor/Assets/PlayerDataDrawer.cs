using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(PlayerData))]
public class PlayerDataDrawer : PropertyDrawer
{

    /// <summary>
    /// PropertyDrawer : ONGUI()에서는 GUILayout / EditorGUILayout류의 자동레이아웃 클래스 X
    /// </summary>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        GUI.Box(position, GUIContent.none, GUI.skin.window);

        //라벨으로 데이터라는 이름을띄움 
        EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        {
            EditorGUI.indentLevel++;

            var rt = new Rect(position.x, position.y + GUIStyle.none.CalcSize(label).y + 2, position.width, 16);

            foreach(SerializedProperty prop in property)
            {
                GUI.color = new Color(Random.value, Random.value, Random.value);
                EditorGUI.PropertyField(rt, prop);
                rt.y += 18;
            }

            GUI.color = Color.white;

            EditorGUI.indentLevel--;
        }
    }

    /// <summary>
    /// 박스의 영역값을 계산 안하면 스크립트 크기가 엄청커짐,, 
    /// </summary>
    
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        int cnt = 0;

        foreach(var prop in property)
        {
            cnt++;
        }

        return EditorGUIUtility.singleLineHeight * (cnt + 1) + 6;
    }
}
