using UnityEngine;

public class TurnStartState : TurnBaseState
{
    public override void Enter(TurnManager manager)
    {
        Debug.Log($"Turn Start : 次のAttack担当 = {manager.GetPlayerName(manager.GetAttackPlayer())}");

        // Turnタイマー初期化
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
