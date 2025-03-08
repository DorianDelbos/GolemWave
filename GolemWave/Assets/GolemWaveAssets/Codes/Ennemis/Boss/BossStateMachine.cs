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

    [SerializeField] Transform player;
    [SerializeField] GameObject projectilePf;

    public Animator AnimatorComp {  get; private set; }
    public Transform Player { get => player; private set => player = value; }
    public GameObject ProjectilePf { get => projectilePf; private set => projectilePf = value; }

    void Start()
    {
        factory = new StateFactory<BossStateMachine>(this);
        currentState = factory.GetState<Phase1>(true);

        AnimatorComp = GetComponent<Animator>();
    }

    void Update()
    {
        if (factory.valid)
            currentState.Update();
    }
}