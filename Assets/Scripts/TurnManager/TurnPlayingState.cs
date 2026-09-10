using UnityEngine;

public class TurnPlayingState : TurnBaseState
{
    public override void Enter(TurnManager manager)
    {
        Debug.Log("Turn Playing State Enter");
    }


    public override void UpdateState(TurnManager manager)
    {
        // TurnŠÔXV
        manager.UpdateTurnTimer();

        // ŠÔØ‚ê
        if (manager.GetTurnTimer() <= 0.0f)
        {
            manager.ChangeState(TurnStateType.TurnEnd);
        }
    }


    public override void Exit(TurnManager manager)
    {

    }
}
