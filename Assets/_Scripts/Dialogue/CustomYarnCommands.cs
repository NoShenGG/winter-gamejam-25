using UnityEngine;
using Yarn.Unity;

[RequireComponent(typeof(DialogueRunner))]
public class CustomYarnCommands : MonoBehaviour {
    private DialogueRunner _dialogueRunner;
    private bool _isFloating;

    public void Awake() {
        _dialogueRunner = GetComponent<DialogueRunner>();
        _dialogueRunner.AddCommandHandler<bool>(
            "set_floating",
            SetFloatingPresenter
        );
    }

    public void Start() {
        GameManager.Instance.DialogueRunner = _dialogueRunner;
        SetFloatingPresenter(false);
    }

    private void SetFloatingPresenter(bool enableFloating) {
        _isFloating = enableFloating;
        foreach (DialoguePresenterBase presenter in _dialogueRunner.DialoguePresenters) {
            if (presenter is BlockLinePresenter blockline) {
                blockline.displayEnable = !_isFloating;
            } else if (presenter is FloatingLinePresenter floating) {
                floating.displayEnable = _isFloating;
            }
        }
    }
}
