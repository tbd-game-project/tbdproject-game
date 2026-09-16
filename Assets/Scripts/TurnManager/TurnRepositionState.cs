using UnityEngine;

public class TurnRepositionState : TurnBaseState
{
    private bool defendEntered;
    private bool attackEntered;


    public override void Enter(TurnManager manager)
    {
        //Debug.Log("Turn Reposition State Enter");

        defendEntered = false;
        attackEntered = false;

        manager.SetTurnTimer(Mathf.Max(manager.GetDefendRepositionTime(), manager.GetAttackRepositionTime()));

        manager.GetPlayer1().StartReposition();
        manager.GetPlayer2().StartReposition();
    }

    public override void UpdateState(TurnManager manager)
    {
        manager.UpdateTurnTimer();

        float totalTime = Mathf.Max(manager.GetDefendRepositionTime(),manager.GetAttackRepositionTime());

        // RepositionŠJŽn‚©‚ç‚ÌŒo‰ßŽžŠÔ
        float elapsedTime = totalTime - manager.GetTurnTimer();

        if (!defendEntered && elapsedTime >= manager.GetDefendRepositionTime()) 
        {
            defendEntered = true;

            manager.GetDefendPlayer().EndReposition();
        }

        if (!attackEntered && elapsedTime >= manager.GetAttackRepositionTime())
        {
            attackEntered = true;

            manager.GetAttackPlayer().EndReposition();

            manager.ChangeState(TurnStateType.TurnStart);
        }
    }

    public override void Exit(TurnManager manager)
    {
    }
}
