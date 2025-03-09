// ---[ STATE ] ---
// Replace "Phase4_STATEMACHINE" by your state machine class name.
//
// Here is an exemple of the CheckSwitchStates method:
// protected override void CheckSwitchStates()
// {
//      if (isRunning)
//      {
//          SwitchState(Factory.GetState<RunningState>());
//      }
// }

using StateMachine; // include all scripts about StateMachines
using UnityEngine;
using UnityEngine.SceneManagement;

public class Phase4 : BaseState<BossStateMachine>
{
    public Phase4(BossStateMachine currentContext, StateFactory<BossStateMachine> currentFactory)
        : base(currentContext, currentFactory) { }

    // This method will be called every Update to check whether or not to switch states.
    protected override void CheckSwitchStates()
    {
        Vector3 headToPlayer = Context.Player.position - Context.crystalTransform.transform.position;

        if (headToPlayer.sqrMagnitude <= 4f * 4f)
        {
            SceneManager.LoadScene("EndScreen");
        }

    }

    // This method will be called only once before the update.
    protected override void EnterState()
    {
        Context.HeadAnimator.ResetTrigger("Open");
        Context.HeadAnimator.SetTrigger("Open");
    }

    // This method will be called only once after the last update.
    protected override void ExitState()
    {
    }

    // This method will be called every frame.
    protected override void UpdateState()
    {

    }

    // This method will be called on state switch.
    // No need to modify this method !
    protected override void SwitchState(BaseState<BossStateMachine> newState)
    {
        base.SwitchState(newState);
        Context.currentState = newState;
    }
}
