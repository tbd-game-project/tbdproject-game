using UnityEngine;

public class TurnEndState : TurnBaseState
{
    public override void Enter(TurnManager manager)
    {
        
    }


    public override void UpdateState(TurnManager manager)
    {
        //if (/*�������f*/)
        //{
        //    manager.EndBattle();
        //
        //    // Result State��
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
                    // 5�̏���
                    Debug.Log($"5��̌��o(������ {matchData.Owner}): �Y���ӏ�[ �N�_:{matchData.Coordinate} / ����{matchData.Direction}]");
                    break;

                case 4:
                    // 4�̏���
                    Debug.Log($"4��̌��o(������ {matchData.Owner}): �Y���ӏ�[ �N�_:{matchData.Coordinate} / ����{matchData.Direction}]");
                    break;

                case 3:
                    // 3�̏���
                    Debug.Log($"3��̌��o(������ {matchData.Owner}): �Y���ӏ�[ �N�_:{matchData.Coordinate} / ����{matchData.Direction}]");
                    break;
            }
        }
        MatchStorage.Instance.ClearMatchData();

        // 5�ȊO�͎���Turn��
        manager.ProceedNextTurn();
    }


    public override void Exit(TurnManager manager)
    {

    }
}
