// ---[ STATE MACHINE ] ---
// "factory" is used to get all possible states
// "currentState" can be set in the start method with : currentState = factory.GetState<YOUR_STATE>();

using UnityEngine;
using StateMachine;
using System.Collections.Generic;

public class BossStateMachine : MonoBehaviour
{
    [HideInInspector]
    public BaseState<BossStateMachine> currentState;
    private StateFactory<BossStateMachine> factory;

    [SerializeField] Transform player;

    [Header("Phase 1")]
    [SerializeField] GameObject projectilePf;

    [Header("Phase 2")]
    [SerializeField] GameObject heelCrystal;
    [SerializeField] GameObject enemyPf;

    [Header("Phase 3")]
    [SerializeField] GameObject[] gravityZones;

    [Header("Phase 3")]
    [SerializeField] List<GameObject> crystals;


    public Animator AnimatorComp { get; private set; }
    public Transform Player { get => player; private set => player = value; }
    public GameObject ProjectilePf { get => projectilePf; private set => projectilePf = value; }
    public GameObject HeelCrystal { get => heelCrystal; private set => heelCrystal = value; }
    public GameObject EnemyPf { get => enemyPf; private set => enemyPf = value; }
    public GameObject[] GravityZones { get => gravityZones; private set => gravityZones = value; }
    public List<GameObject> Crystals { get => crystals; private set => crystals = value; }

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