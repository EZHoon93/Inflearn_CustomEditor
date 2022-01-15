using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EditorHelper 
{
    public static void DrawCenterLabel(GUIContent content, Color color , int size , FontStyle style)
    {
        var guiStyle = new GUIStyle();
        guiStyle.fontSize = size;
        guiStyle.fontStyle = style;
        guiStyle.normal.textColor = color;
        guiStyle.padding.top = 10;
        guiStyle.padding.bottom = 10;

        GUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            GUILayout.Label(content, guiStyle);
            GUILayout.FlexibleSpace();
            //2개를 양옆에 띄움으로써 라벨을 센터에 ,,          FlexiableSpace// Label  // FlexibleSpcae 
        }

        GUILayout.EndHorizontal();


    }

    public static bool DrawCenterButton(string text , Vector2 size)
    {
        bool clicked;


        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            clicked = GUILayout.Button(text, GUILayout.Width(size.x), GUILayout.Height(size.y));

            GUILayout.FlexibleSpace();

        }
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        return clicked;
    }

    /// <summary>
    /// 공식은 인터넷:?에나와잇음 충돌포지션 갖고오기.
    /// </summary>
    public static   void RayCast(Vector3 rayOriPos , Vector3 rayDestPos , out Vector3 hitPos)
    {
        Vector3 planePos01 = Vector3.up;
        Vector3 planePos02 = Vector3.right;
        Vector3 planePos03 = Vector3.down;

        Vector3 planeDir = Vector3.Cross((planePos02 - planePos01).normalized, (planePos03 - planePos01).normalized); //법선벡터구하기
        Vector3 lineDir = (rayDestPos - rayOriPos).normalized;

        float dotLinePlane = Vector3.Dot(lineDir, planeDir);
        float t = Vector3.Dot(planePos01 - rayOriPos, planeDir) / dotLinePlane;

        hitPos = rayOriPos + (lineDir * t);
    }

    public static Vector2 DrawGridItems(Vector2 scrollPos , int gapSpace, int itemCnt, float areaWidth , Vector2 slotSize, System.Action<int> onDrawer)
    {
        scrollPos = GUILayout.BeginScrollView(scrollPos);
        {
            int horCnt = (int)(areaWidth / slotSize.x);
            if(horCnt <= 0)
            {
                horCnt = 1;
            }

            int verCnt = itemCnt / horCnt;
            if(itemCnt % horCnt > 0)
            {
                verCnt++;
            }
            if(verCnt <= 0)
            {
                verCnt = 1;
            }
            GUILayout.BeginVertical();
            {
                for(int i = 0; i < verCnt; i++)
                {
                    GUILayout.BeginHorizontal();
                    {
                        for(int j = 0; j < horCnt; j++)
                        {
                            int idx = j + (i * horCnt);
                            if(idx >= itemCnt)
                            {
                                break;
                            }
                            Debug.Log(idx + "idx!!");
                            onDrawer(idx);

                            GUILayout.Space(gapSpace);
                        }
                    }
                    GUILayout.EndHorizontal();
                }
            }
        }

        GUILayout.EndScrollView();

        return scrollPos;
    }
}
