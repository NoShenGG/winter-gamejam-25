using System;
using System.Collections;
using UnityEngine;
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

    [YarnCommand("set_emotion")]
    public void SetEmotion(string emotion) {
        switch (emotion) {
            default:
                Debug.LogErrorFormat("Emotion \"{0}\" is invalid!", emotion);
                break;
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
