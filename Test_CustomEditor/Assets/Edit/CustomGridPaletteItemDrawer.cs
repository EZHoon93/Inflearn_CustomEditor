using System.Collections;
using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

public class CustomGridPaletteItemDrawer  
{
    public static bool Draw(Vector2 slotSize , bool isSelected, CustomGridPaletteItem item)
    {
        //실제로먼가그려지지않지만 할당된것만큼 크기를가짐
        var area = GUILayoutUtility.GetRect(slotSize.x , slotSize.y , GUIStyle.none ,GUILayout.MaxWidth(slotSize.x) , GUILayout.MaxHeight(slotSize.y) );

        bool selected = GUI.Button(area, AssetPreview.GetAssetPreview(item.targetObject));

        GUI.Label(new Rect(area.center.x , area.center.y , 100, 50) , item.name);

        if (isSelected)
        {
            var selectMarkArea = area;
            selectMarkArea.x = selectMarkArea.center.x - 30;
            selectMarkArea.width = 30;
            selectMarkArea.height = 30;
            GUI.DrawTexture(selectMarkArea, EditorGUIUtility.FindTexture("d_FilterSelectedOnly@2x"));

        }
        //버튼이 눌렷는지안눌렷늕니
        return selected;
    }
}
