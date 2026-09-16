using UnityEngine;

public class TurnEndState : TurnBaseState
{
    public override void Enter(TurnManager manager)
    {
        //Debug.Log($"Turn End : {manager.GetPlayerName(manager.GetAttackPlayer())} のAttackターン終了");
    }


    public override void UpdateState(TurnManager manager)
    {
        //if (/*勝負判断*/)
        //{
        //    manager.EndBattle();
        //
        //    // Result Stateへ
        //    manager.ChangeState(TurnStateType.Result);
        //
        //    return;
        //}


        int matchCount = MatchStorage.Instance.GetMatchCount();
        if (matchCount > 0)
        {
            MatchStorage.Instance.SortMatchData();
        }
        for(int i = 0; i < matchCount; i++)
        {
            MatchData matchData = MatchStorage.Instance.PopMatchData();

            switch (matchData.Num)
            {
                case 5:
                    // 5の処理
                    Debug.Log($"5列の検出(所持者 {matchData.Owner}): 該当箇所[ 起点:{matchData.Coordinate} / 方向{matchData.Direction}]");
                    break;

                case 4:
                    // 4の処理
                    Debug.Log($"4列の検出(所持者 {matchData.Owner}): 該当箇所[ 起点:{matchData.Coordinate} / 方向{matchData.Direction}]");
                    break;

                case 3:
                    // 3の処理
                    Debug.Log($"3列の検出(所持者 {matchData.Owner}): 該当箇所[ 起点:{matchData.Coordinate} / 方向{matchData.Direction}]");
                    break;
            }
        }
        MatchStorage.Instance.ClearMatchData();

        // 5以外は次のTurnへ
        manager.ProceedNextTurn();
    }


    public override void Exit(TurnManager manager)
    {

    }
}
