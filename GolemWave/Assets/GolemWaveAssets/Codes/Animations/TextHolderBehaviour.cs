using UnityEngine;

namespace GolemWave
{
    public class TextHolderBehaviour : StateMachineBehaviour
    {
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Destroy(animator.gameObject);
        }
    }
}
