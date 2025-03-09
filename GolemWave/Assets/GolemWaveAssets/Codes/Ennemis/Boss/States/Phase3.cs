// ---[ STATE ] ---
// Replace "Phase3_STATEMACHINE" by your state machine class name.
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

public class Phase3 : BaseState<BossStateMachine>
{
    public Phase3(BossStateMachine currentContext, StateFactory<BossStateMachine> currentFactory)
        : base(currentContext, currentFactory) { }

    // This method will be called every Update to check whether or not to switch states.
    protected override void CheckSwitchStates()
    {
        if (Context.Crystals.Count <= 0)
        {
            SwitchState(Factory.GetState<Phase4>());
        }
        else
        {
            Context.Crystals.RemoveAll(x => !x);
        }
    }

    // This method will be called only once before the update.
    protected override void EnterState()
    {
        //foreach (var gravityZone in Context.GravityZones)
        //{
        //    gravityZone.SetActive(true);
        //}
        Context.transform.GetChild(0).position -= Context.transform.up * 5f;
    }

    // This method will be called only once after the last update.
    protected override void ExitState()
    {
        Debug.Log("fini");
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
