using UnityEngine;
using Yarn.Unity;

public class FloatingLinePresenter : DialoguePresenterBase {
    [SerializeField] private CanvasGroup _canvasGroup;
    public bool displayEnable = false;
    

    public override YarnTask OnDialogueCompleteAsync() {
        if (_canvasGroup != null) {
            _canvasGroup.alpha = 0;
        }
        return YarnTask.CompletedTask;
    }

    public override YarnTask OnDialogueStartedAsync() {
        if (_canvasGroup != null) {
            _canvasGroup.alpha = 0;
        }
        return YarnTask.CompletedTask;
    }

    public override async YarnTask RunLineAsync(LocalizedLine line, LineCancellationToken token) {
        if (!displayEnable) {
            return;
        }

        if (line.CharacterName == null) {
            Debug.LogError($"Cannot associate floating text box with null character in dialogue line: {line.Text}");
            return;
        }

        
    }
}
