using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStateIdle", menuName = "PlayerState/Idle")]
public class PlayerStateIdle : PlayerState
{
    public override void EnterState(Player owner, PlayerInputReader input)
    {
        base.EnterState(owner, input);
    }

    public override void UpdateState()
    {
        base.UpdateState();

        // “ü—Í‚ª‚ ‚ê‚ÎˆÚ“®ó‘Ô‚É‘JˆÚ
        if (input.MoveValue != Vector2.zero)
        {
            owner.ChangeState("move");
        }
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
