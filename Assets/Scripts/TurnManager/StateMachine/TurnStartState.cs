using UnityEngine;

public class TurnStartState : TurnBaseState
{
    public override void Enter(TurnManager manager)
    {
        // Turn�^�C�}�[������
        manager.ResetTurnTimer();
    }


    public override void UpdateState(TurnManager manager)
    {
        manager.ChangeState(TurnStateType.Playing);
    }


    public override void Exit(TurnManager manager)
    {

    }
}
