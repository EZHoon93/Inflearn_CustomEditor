using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGrid : MonoBehaviour
{
    public CustomGridConfig config;
    public Dictionary<Vector2Int, MapObject> items = new Dictionary<Vector2Int, MapObject>();
    public Vector3[] horLines;
    public Vector3[] verLines;

    public bool reposition;

    //에디터 값이 수정될때 호출
    private void OnValidate()
    {
        reposition = true;
    }

    public Vector2Int GetCellPos(Vector3 worldPos)
    {
        //내림처리
        return new Vector2Int(Mathf.FloorToInt(worldPos.x / config.CellSize.x), Mathf.FloorToInt(worldPos.y / config.CellSize.y));
    }

    public void RefreshPoints()
    {
        int verLineCnt = config.CellCount.x * 2 + 2;
        int horLineCnt = config.CellCount.y * 2 + 2;

        horLines = new Vector3[horLineCnt];
        verLines= new Vector3[verLineCnt];

        for(int i = 0; i <= config.CellCount.x; i++)
        {
            verLines[i * 2] = new Vector3(i * config.CellSize.x, config.CellSize.y * config.CellCount.y, 0);
            verLines[i * +2 + 1] = new Vector3(i * config.CellSize.x, 0, 0);

        }

        for (int i = 0; i <= config.CellCount.y; i++)
        {
            horLines[i * 2] = new Vector3(0, i * config.CellSize.y, 0);
            horLines[i * 2 + 1] = new Vector3(config.CellSize.x * config.CellCount.x, i * config.CellSize.y, 0);
        }


    }

    public bool Contains(Vector2Int cellPos)
    {
        return cellPos.x >= 0 && cellPos.x < config.CellCount.x && cellPos.y >= 0 && cellPos.y < config.CellCount.y;
    }

    public bool IsItemExist(Vector2Int cellPos)
    {
        return items.ContainsKey(cellPos);
    }

    public MapObject GetItem(Vector2Int cellPos)
    {
        if(items.ContainsKey(cellPos) == false)
        {
            return null;
        }

        return items[cellPos];
    }

    public void RemoveItem(Vector2Int cellPos)
    {
        if (items.ContainsKey(cellPos))
        {
            items.Remove(cellPos);
        }
    }

    public MapObject  AddItem(Vector2Int cellPos, CustomGridPaletteItem paletteItem)
    {
        if (items.ContainsKey(cellPos))
        {
            Debug.LogError("Error");
            return null;
        }

        var target = GameObject.Instantiate(paletteItem.targetObject, transform);
        target.transform.position = GetWorldPos(cellPos);
        var comp = target.AddComponent<MapObject>();
        comp.id = paletteItem.id;
        comp.cellPos = cellPos;

        items.Add(cellPos, comp);
        return comp;
    }

    public Vector3 GetWorldPos(Vector2Int cellPos)
    {
        return new Vector3(cellPos.x * config.CellSize.x + config.CellSize.x * 0.5f, cellPos.y * config.CellSize.y + config.CellSize.y * 0.5f, 0);
    }

}
