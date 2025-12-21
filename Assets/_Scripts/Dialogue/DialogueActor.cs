using System;
using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;
using Yarn.Markup;
using Yarn.Unity;

[RequireComponent(typeof(Animator))]
public class DialogueActor : MonoBehaviour {
    #region Animation

    private Animator _animator;
    private bool _triggerAnimationEnd = false;

    #endregion

    private void Awake() {
        _animator = GetComponent<Animator>();
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
