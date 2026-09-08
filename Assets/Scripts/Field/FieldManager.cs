using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    private Vector2Int fieldSize = new(10, 10);
    private List<FieldTile> FieldTile = new();

    private Vector2Int[] checkDirection = new Vector2Int[4]
    {
        new Vector2Int(1, 0), // 横方向
        new Vector2Int(0, 1), // 縦方向
        new Vector2Int(1, 1), // 斜め方向（右上）
        new Vector2Int(1, -1) // 斜め方向（右下）
    };

    // ５目生後判定用の配列、ゲーム中の盤面アクセス用
    private FieldTile[,] FieldTileArray;

    public void BeginFieldSetup(Vector2Int size)
    {
        fieldSize = size;
        FieldTile.Clear();

        FieldTileArray = new FieldTile[fieldSize.x, fieldSize.y];
    }

    public void RegisterFieldTile(Vector2Int coodinate, FieldTile piece)
    { 
        if(!IsInsideField(coodinate))
        {
            Debug.LogError($"[FieldManager] RegisterFieldTile: 範囲外の指定です Coodinate:{coodinate} / FieldSize:{fieldSize}");
            return;
        }
        FieldTile.Add(piece);
        FieldTileArray[coodinate.x, coodinate.y] = piece;

        piece.SetCoodinate(coodinate.x, coodinate.y);
    }

    public FieldTile GetFieldTile(Vector2Int coodinate)
    {
        if(!IsInsideField(coodinate))
        {
            Debug.LogError($"[FieldManager] GetFieldTile: 範囲外の指定です Coodinate:{coodinate} / FieldSize:{fieldSize}");
            return null;
        }

        FieldTile ret = FieldTileArray[coodinate.x, coodinate.y];

        return ret;
    }

    public bool CheckFiveLine(Vector2Int currentCoodinate)
    {
        if(!IsInsideField(currentCoodinate))
        {
            Debug.LogError($"[FieldManager] CheckFiveLine: 範囲外の指定です Coodinate:{currentCoodinate} / FieldSize:{fieldSize}");
            return false;
        }

        foreach (Vector2Int num in checkDirection)
        {

        }
        return true;
    }

    private bool IsInsideField(Vector2Int coodinate)
    {
        return coodinate.x >= 0 && coodinate.x < fieldSize.x && coodinate.y >= 0 && coodinate.y < fieldSize.y;
    }
}
