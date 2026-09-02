using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatePlace", menuName = "PlayerState/Place")]
public class PlayerStatePlace : PlayerState
{
    public override void EnterState(Player owner, PlayerInputReader input)
    {
        base.EnterState(owner, input);

        // TO DO:İ’uó‘Ô‚É“ü‚éÛ‚Ìˆ—‚ğ’Ç‰Á
        if(owner.OnStandingPiece == null)
        {
            owner.ChangeState("idle");
            return;
        }

        Stone stone = new Stone();
        stone.SetOwner(owner);

        Instantiate(stone);
        owner.OnStandingPiece.PutStone(stone);
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
