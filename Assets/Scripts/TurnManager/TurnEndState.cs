using UnityEngine;

public class TurnEndState : TurnBaseState
{
    public override void Enter(TurnManager manager)
    {
        Debug.Log($"Turn End : {manager.GetPlayerName(manager.GetAttackPlayer())} のAttackターン終了");
    }


    public override void UpdateState(TurnManager manager)
    {
        // ==============================
        // 勝敗判定
        // ==============================

        // 勝負が決まった
        //if (/*勝負判断*/)
        //{
        //    manager.EndBattle();
        //
        //    // Result Stateへ
        //    manager.ChangeState(TurnStateType.Result);
        //
        //    return;
        //}


        // ==============================
        // 次Turn準備
        // ==============================
        manager.NextTurn();

        // 次Turn開始
        manager.ChangeState(TurnStateType.TurnStart);
    }


    public override void Exit(TurnManager manager)
    {

    }
}
