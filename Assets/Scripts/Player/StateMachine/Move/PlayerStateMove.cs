using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStateMove", menuName = "PlayerState/Move")]
public class PlayerStateMove : PlayerState
{
    [SerializeField] private float moveSpeed = 5f;

    public override void EnterState(Player owner, PlayerInputReader input)
    {
        base.EnterState(owner, input);
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (input.MoveValue == Vector2.zero)
        {
            owner.ChangeState("idle");
        }
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();

        var controller = owner.GetComponent<CharacterController>();
        var moveDirection = new Vector3(input.MoveValue.x, 0.0f, input.MoveValue.y);

        controller.Move(moveDirection * moveSpeed * Time.fixedDeltaTime);
    }

    public override void ExitState()
    {
        base.ExitState();
        var rb = owner.GetComponent<Rigidbody>();

        rb.linearVelocity = Vector3.zero;
    }
}
