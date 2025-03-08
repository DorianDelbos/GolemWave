// ---[ STATE ] ---
// Replace "Phase1_STATEMACHINE" by your state machine class name.
//
// Here is an exemple of the CheckSwitchStates method:
// protected override void CheckSwitchStates()
// {
//      if (isRunning)
//      {
//          SwitchState(Factory.GetState<RunningState>());
//      }
// }

using StateMachine;
using System.Collections;
using UnityEngine; // include all scripts about StateMachines

public class Phase1 : BaseState<BossStateMachine>
{
    public Phase1(BossStateMachine currentContext, StateFactory<BossStateMachine> currentFactory)
        : base(currentContext, currentFactory) { }

    Coroutine throwProjectilesCoroutine = null;

    protected override void CheckSwitchStates()
    {
        Vector3 alignedPlayerPos = Context.Player.position;
        alignedPlayerPos.y = Context.transform.position.y;
        Vector3 posToPlayer = alignedPlayerPos - Context.transform.position;

        if (posToPlayer.sqrMagnitude <= 30 * 30 && throwProjectilesCoroutine == null)
        {
            SwitchState(Factory.GetState<Phase2>());
        }
    }

    protected override void EnterState()
    {

    }

    protected override void ExitState()
    {

    }

    protected override void UpdateState()
    {

        if (throwProjectilesCoroutine == null)
        {
            throwProjectilesCoroutine = Context.StartCoroutine(ThrowProjectiles());
        }
    }

    // This method will be called on state switch.
    // No need to modify this method !
    protected override void SwitchState(BaseState<BossStateMachine> newState)
    {
        base.SwitchState(newState);
        Context.currentState = newState;
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////

    IEnumerator ThrowProjectiles()
    {
        yield return new WaitForSeconds(2f);

        Context.AnimatorComp.ResetTrigger("Jump");
        Context.AnimatorComp.SetTrigger("Jump");

        yield return new WaitForSeconds(5.5f);

        throwProjectilesCoroutine = null;
    }
}
