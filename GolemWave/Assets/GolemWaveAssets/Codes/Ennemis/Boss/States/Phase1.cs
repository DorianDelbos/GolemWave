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
        //foreach (var gravityZone in Context.GravityZones)
        //{
        //    gravityZone.SetActive(false);
        //}
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

        yield return new WaitForSeconds(3f);

        int spawnAmount = 20;
        float angleStep = 180f / (spawnAmount - 1);
        for (int i = 0; i < spawnAmount; i++)
        {
            float angle = -90f + i * angleStep;
            Quaternion baseRotation = Context.transform.rotation * Quaternion.Euler(0, angle, 0);
            Quaternion randomRotation = Quaternion.Euler(Random.Range(-10f, 10f), Random.Range(-30f, 30f), 0);
            Quaternion finalRotation = baseRotation * randomRotation;

            Vector3 spawnPosition = Context.transform.position + finalRotation * Vector3.forward * Random.Range(20f, 50f);
            spawnPosition.y = 5;

            float targetAngle = Random.Range(0f, 360f);
            float targetRadius = Random.Range(1f, 50f);
            Vector3 targetPosition = Context.Player.position + new Vector3(
                Mathf.Cos(targetAngle * Mathf.Deg2Rad) * targetRadius,
                0,
                Mathf.Sin(targetAngle * Mathf.Deg2Rad) * targetRadius
            );

            GameObject projectile = GameObject.Instantiate(Context.ProjectilePf, spawnPosition, finalRotation);
            projectile.GetComponent<ProjectileCollision>().Initialize(targetPosition);
        }

        yield return new WaitForSeconds(2.5f);
        throwProjectilesCoroutine = null;
    }



}
