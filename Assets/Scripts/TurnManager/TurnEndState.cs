using UnityEngine;

public class TurnEndState : TurnBaseState
{
    public override void Enter(TurnManager manager)
    {
        Debug.Log($"Turn End : {manager.GetPlayerName(manager.GetAttackPlayer())} のAttackターン終了");
    }


    public override void UpdateState(TurnManager manager)
    {

        // 5以外は次のTurnへ
        manager.ProceedNextTurn();
    }


    public override void Exit(TurnManager manager)
    {

    }
}
