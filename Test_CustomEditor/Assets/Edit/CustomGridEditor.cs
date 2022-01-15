using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(CustomGrid))]

public class CustomGridEditor : Editor
{
    [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.InSelectionHierarchy)]
    static void DrawHandle(CustomGrid grid, GizmoType type)
    {
        if (grid.reposition)
        {
            grid.RefreshPoints();
            grid.reposition = false;
        }

        Handles.DrawLines(grid.verLines);
        Handles.DrawLines(grid.horLines);

    }
}