using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStateReposition", menuName = "PlayerState/Reposition")]
public class PlayerStateReposition : PlayerState
{
    [SerializeField] private float moveSpeed = 5f;

    public override void EnterState(Player owner, PlayerInputReader input)
    {
        Debug.Log("PlayerStateReposition: EnterState");

        base.EnterState(owner, input);

        //アウト演出モデル切り替え
        owner.EnterRepositionVisual();
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();

        var rb = owner.GetComponent<Rigidbody>();
        var moveDirection = new Vector3(input.MoveValue.x, 0.0f, input.MoveValue.y);

        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }

    public override void ExitState()
    {
        base.ExitState();
        var rb = owner.GetComponent<Rigidbody>();

        rb.linearVelocity = Vector3.zero;

        //イン演出モデル切り替え
        owner.ExitRepositionVisual();
    }
}
