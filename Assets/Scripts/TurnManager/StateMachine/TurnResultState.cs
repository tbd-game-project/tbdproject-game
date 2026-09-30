using UnityEngine;

public class TurnResultState : TurnBaseState
{
    public override void Enter(TurnManager manager)
    {
        manager.GetPlayer1().SetCanPlaceStone(false);
        manager.GetPlayer2().SetCanPlaceStone(false);
    }


    public override void UpdateState(TurnManager manager)
    {        
        FieldManager.Instance.HighlightAllStoneColor();
    }


    public override void Exit(TurnManager manager)
    {

    }
}
