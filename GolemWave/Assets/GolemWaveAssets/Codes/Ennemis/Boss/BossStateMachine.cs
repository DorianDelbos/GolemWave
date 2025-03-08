// ---[ STATE MACHINE ] ---
// "factory" is used to get all possible states
// "currentState" can be set in the start method with : currentState = factory.GetState<YOUR_STATE>();

using UnityEngine;
using StateMachine; // include all script about stateMachine

public class BossStateMachine : MonoBehaviour
{
    [HideInInspector]
    public BaseState<BossStateMachine> currentState;
    private StateFactory<BossStateMachine> factory;

    void Start()
    {
        factory = new StateFactory<BossStateMachine>(this);
        currentState = factory.GetState<Phase1>(true);
    }

    void Update()
    {
        if (factory.valid)
            currentState.Update();
    }
}