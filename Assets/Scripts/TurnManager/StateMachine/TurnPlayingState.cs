using UnityEngine;

public class TurnPlayingState : TurnBaseState
{
    public override void Enter(TurnManager manager)
    {
        
    }


    public override void UpdateState(TurnManager manager)
    {
        // Turn���ԍX�V
        manager.UpdateTurnTimer();

        // ���Ԑ؂�
        if (manager.GetTurnTimer() <= 0.0f)
        {
            manager.ChangeState(TurnStateType.TurnEnd);
        }
    }


    public override void Exit(TurnManager manager)
    {

    }
}
