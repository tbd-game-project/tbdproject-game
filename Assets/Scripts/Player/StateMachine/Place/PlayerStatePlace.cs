using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatePlace", menuName = "PlayerState/Place")]
public class PlayerStatePlace : PlayerState
{
    [Header("Place State Settings")]
    [SerializeField] private GameObject stonePrefab; // 石のプレハブ
    [SerializeField] private float rayPositionOffset = -0.5f; // レイの開始位置のオフセット
    [SerializeField] private float rayDistance = 1.0f; // レイの距離
    [SerializeField] private LayerMask fieldLayer; // フィールドのレイヤーマスク

    public override void EnterState(Player owner, PlayerInputReader input)
    {
        base.EnterState(owner, input);

        Debug.Log("PlayerStatePlace: EnterState");

        owner.TryGetFieldPieceBelow(rayPositionOffset, rayDistance, fieldLayer);

        if (owner.OnStandingPiece == null)
        {
            owner.ChangeState("idle");
            return;
        }

        if(owner.OnStandingPiece.CanPutStone())
        { // 足元の床に石が置ける状態
            Stone stone = Instantiate(stonePrefab).GetComponent<Stone>();
            stone.SetOwner(owner);
            owner.OnStandingPiece.PutStone(stone);
            owner.ChangeState("idle");
            return;
        }
        else
        { // 足元の床に石が置けない状態
            owner.ChangeState("idle");
            return;
        }
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }
}
