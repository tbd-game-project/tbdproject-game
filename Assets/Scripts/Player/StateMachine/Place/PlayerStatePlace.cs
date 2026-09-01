using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatePlace", menuName = "PlayerState/Place")]
public class PlayerStatePlace : PlayerState
{
    public override void EnterState(Player owner, PlayerInputReader input)
    {
        base.EnterState(owner, input);

        // TO DO:İ’uó‘Ô‚É“ü‚éÛ‚Ìˆ—‚ğ’Ç‰Á
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
