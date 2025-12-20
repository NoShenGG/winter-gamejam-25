using UnityEngine;

public class SignalAnimationEnd : StateMachineBehaviour {
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        DialogueActor actor;
        if (animator.gameObject.TryGetComponent<DialogueActor>(out actor)) {
            actor.OnAnimationEnd();
        }
    }
}
