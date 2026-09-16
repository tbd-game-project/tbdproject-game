using UnityEngine;

public class TurnEndState : TurnBaseState
{
    public override void Enter(TurnManager manager)
    {
        
    }


    public override void UpdateState(TurnManager manager)
    {

        // 5ˆÈŠO‚ÍŽŸ‚ÌTurn‚Ö
        manager.ProceedNextTurn();
    }


    public override void Exit(TurnManager manager)
    {

    }
}
