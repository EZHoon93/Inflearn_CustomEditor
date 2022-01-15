using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGridPaletteDrawer  
{

    public CustomGridPalette TargetPalette;
    Vector2 slotSize = new Vector2(100, 100);
    Vector2 scrollPos;

    int selectedIdx;

    public CustomGridPaletteItem SelectedItem
    {
        get
        {
            if(selectedIdx == -1)
            {
                return null;
            }
            return TargetPalette.items[selectedIdx];
        }
    }

    //실제그리는곳
    public void Draw(Vector2 winSize)
    {
        if(TargetPalette == null || TargetPalette.items.Count == 0)
        {
            EditorHelper.DrawCenterLabel(new GUIContent("데이터없음"), Color.red, 20, FontStyle.Bold  );
            return;
        }

        if(selectedIdx == -1)
        {
            selectedIdx = 0;
        }
        if (selectedIdx >= TargetPalette.items.Count-1)
        {
            selectedIdx = 0;
        }
        scrollPos = EditorHelper.DrawGridItems(scrollPos, 10, TargetPalette.items.Count, winSize.x, slotSize , (idx) =>
        {
            Debug.Log(idx + "/" + TargetPalette.items.Count);

            bool selected = CustomGridPaletteItemDrawer.Draw(slotSize,selectedIdx == idx, TargetPalette.items[idx]);
            if (selected)
            {
                selectedIdx = idx;
            }
        });
    }
}
