// ---[ STATE ] ---
// Replace "Phase2_STATEMACHINE" by your state machine class name.
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

public class Phase2 : BaseState<BossStateMachine>
{
    public Phase2(BossStateMachine currentContext, StateFactory<BossStateMachine> currentFactory)
        : base(currentContext, currentFactory) { }
        
    // This method will be called every Update to check whether or not to switch states.
    protected override void CheckSwitchStates()
    {
        throw new System.NotImplementedException();
    }

    // This method will be called only once before the update.
    protected override void EnterState()
    {
        throw new System.NotImplementedException();
    }

    // This method will be called only once after the last update.
    protected override void ExitState()
    {
        throw new System.NotImplementedException();
    }

    // This method will be called every frame.
    protected override void UpdateState()
    {
        throw new System.NotImplementedException();
    }

    // This method will be called on state switch.
    // No need to modify this method !
    protected override void SwitchState(BaseState<BossStateMachine> newState)
    {
        base.SwitchState(newState);
        Context.currentState = newState;
    }
}
