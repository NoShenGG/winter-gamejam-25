using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

[RequireComponent(typeof(Animator))]
public class DialogueActor : MonoBehaviour {
    #region Animation

    private Animator _animator;
    private bool _triggerAnimationEnd = false;

    #endregion

    public string ActorName;

    private void Awake() {
        _animator = GetComponent<Animator>();

        if (ActorName == null) {
            Debug.LogError($"\"{gameObject.name}\" attempted to register with null ActorName");
        } else if (ActorName.Equals("")) {
            Debug.LogError($"\"{gameObject.name}\" attempted to register with empty ActorName");
        }
    }

    private void Start() {
        Dictionary<string, DialogueActor> actors = GameManager.Instance.DialogueActors;
        if (!actors.TryAdd(ActorName, this)) {
            Debug.LogError($"\"{gameObject.name}\" attempted to register with duplicate ActorName");
            return;
        }
    }

    [YarnCommand("play_anim")]
    public IEnumerator PlayAnimation(string animation) {
        _triggerAnimationEnd = false;
        try {
            _animator.ResetTrigger(animation);
            _animator.SetTrigger(animation);
        } catch (Exception ex) {
            Debug.Log(ex);
        }

        yield return new WaitUntil(() => _triggerAnimationEnd);
    }
    public void OnAnimationEnd() {
        _triggerAnimationEnd = true;
    }
}
