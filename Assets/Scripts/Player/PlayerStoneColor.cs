using UnityEngine;

/// <summary>
/// このプレイヤーが配置する駒の表示色を設定する部品。
///
/// 操作方法や駒の配置処理は変更せず、色の情報だけを持つ。
/// 同じチームのプレイヤーには、Inspectorで同じ色を設定する。
/// </summary>
public class PlayerStoneColor : MonoBehaviour
{
    [Header("配置する駒の色")]

    [Tooltip("このプレイヤーが置いた直後の駒の色。同じチームでは同じ色に設定する。")]
    [SerializeField] private Color stoneColor = Color.red;

    /// <summary>
    /// ほかの処理が、このプレイヤーの駒の色を取得するための窓口。
    /// 外部からの書き換えは許可せず、設定はInspectorで行う。
    ///
    /// この色は見た目用であり、勝敗判定用のチームIDではない。
    /// </summary>
    public Color StoneColor => stoneColor;
}