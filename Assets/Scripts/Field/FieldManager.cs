using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    private Vector2Int fieldSize = new(10, 10);
    private List<FieldPiece> fieldPiece = new();

    private Vector2Int[] checkDirection = new Vector2Int[4]
    {
        new Vector2Int(1, 0), // 横方向
        new Vector2Int(0, 1), // 縦方向
        new Vector2Int(1, 1), // 斜め方向（右上）
        new Vector2Int(1, -1) // 斜め方向（右下）
    };

    // ５目生後判定用の配列、ゲーム中の盤面アクセス用
    private FieldPiece[,] fieldPieceArray;

    public void BeginFieldSetup(Vector2Int size)
    {
        fieldSize = size;
        fieldPiece.Clear();

        fieldPieceArray = new FieldPiece[fieldSize.x, fieldSize.y];
    }

    public void RegisterFieldPiece(Vector2Int coodinate, FieldPiece piece)
    { 
        if(!IsInsideField(coodinate))
        {
            Debug.LogError($"[FieldManager] RegisterFieldPiece: 範囲外の指定です Coodinate:{coodinate} / FieldSize:{fieldSize}");
            return;
        }
        fieldPiece.Add(piece);
        fieldPieceArray[coodinate.x, coodinate.y] = piece;

        piece.SetCoodinate(coodinate.x, coodinate.y);
    }

    public FieldPiece GetFieldPiece(Vector2Int coodinate)
    {
        if(!IsInsideField(coodinate))
        {
            Debug.LogError($"[FieldManager] GetFieldPiece: 範囲外の指定です Coodinate:{coodinate} / FieldSize:{fieldSize}");
            return null;
        }

        FieldPiece ret = fieldPieceArray[coodinate.x, coodinate.y];

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
