using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorStateListener : StateMachineBehaviour {

	string GetEventValue(Animator animator, AnimatorStateInfo stateInfo) {
		return animator.gameObject.name + "/" + stateInfo.shortNameHash;
	}

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		EventManager.TriggerEvent(EventName.AnimatorStateEntered, GetEventValue(animator, stateInfo));
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		EventManager.TriggerEvent(EventName.AnimatorStateExited, GetEventValue(animator, stateInfo));
    }
}
