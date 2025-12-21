using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Yarn.Markup;
using Yarn.Unity;
using Yarn.Unity.Attributes;
using static BlockLinePresenter;

public class FloatingLinePresenter : DialoguePresenterBase {
    internal enum TypewriterType {
        Instant, ByLetter, ByWord,
    }

    private Camera _camera;

    public CanvasGroup canvasGroup;
    public TMP_Text lineText;
    public bool useFadeEffect = true;
    public float fadeUpDuration = 0.25f;
    public float fadeDownDuration = 0.1f;
    public float autoAdvanceDelay = 1f;

    // typewriter fields

    [Group("Typewriter")]
    [SerializeField] internal TypewriterType typewriterStyle = TypewriterType.ByLetter;

    /// <summary>
    /// The number of characters per second that should appear during a
    /// typewriter effect.
    /// </summary>
    [Group("Typewriter")]
    [ShowIf(nameof(typewriterStyle), TypewriterType.ByLetter)]
    [Label("Letters per Second")]
    [Min(0)]
    public int lettersPerSecond = 60;

    [Group("Typewriter")]
    [ShowIf(nameof(typewriterStyle), TypewriterType.ByWord)]
    [Label("Words per Second")]
    [Min(0)]
    public int wordsPerSecond = 10;

    /// <summary>
    /// A list of <see cref="ActionMarkupHandler"/> objects that will be
    /// used to handle markers in the line.
    /// </summary>
    [Group("Typewriter")]
    [Label("Event Handlers")]
    [UnityEngine.Serialization.FormerlySerializedAs("actionMarkupHandlers")]
    [SerializeField] List<ActionMarkupHandler> eventHandlers = new List<ActionMarkupHandler>();
    private List<IActionMarkupHandler> ActionMarkupHandlers {
        get {
            var pauser = new PauseEventProcessor();
            List<IActionMarkupHandler> ActionMarkupHandlers = new()
            {
                pauser,
            };
            ActionMarkupHandlers.AddRange(eventHandlers);
            return ActionMarkupHandlers;
        }
    }

    public bool displayEnable = false;

    private void Awake() {
        switch (typewriterStyle) {
            case TypewriterType.Instant:
                Typewriter = new InstantTypewriter() {
                    ActionMarkupHandlers = ActionMarkupHandlers,
                    Text = this.lineText,
                };
                break;

            case TypewriterType.ByLetter:
                Typewriter = new LetterTypewriter() {
                    ActionMarkupHandlers = ActionMarkupHandlers,
                    Text = this.lineText,
                    CharactersPerSecond = this.lettersPerSecond,
                };
                break;

            case TypewriterType.ByWord:
                Typewriter = new WordTypewriter() {
                    ActionMarkupHandlers = ActionMarkupHandlers,
                    Text = this.lineText,
                    WordsPerSecond = this.wordsPerSecond,
                };
                break;
        }
    }

    private void Start() {
        _camera = Camera.main;
    }

    public override YarnTask OnDialogueCompleteAsync() {
        if (canvasGroup != null) {
            canvasGroup.alpha = 0;
        }
        return YarnTask.CompletedTask;
    }

    public override YarnTask OnDialogueStartedAsync() {
        if (canvasGroup != null) {
            canvasGroup.alpha = 0;
        }
        return YarnTask.CompletedTask;
    }

    public override async YarnTask RunLineAsync(LocalizedLine line, LineCancellationToken token) {
        if (!displayEnable) {
            return;
        }

        if (line.CharacterName == null) {
            Debug.LogError($"Cannot associate floating text box with null character name in dialogue line: \"{line.Text}\"");
            return;
        } else if (line.CharacterName.Length == 0) {
            Debug.LogError($"Cannot associate floating text box with empty character name in dialogue line: \"{line.Text}\"");
            return;
        }

        Dictionary<string, DialogueActor> actors = GameManager.Instance.DialogueActors;
        DialogueActor actor;
        if (!actors.TryGetValue(line.CharacterName, out actor)) {
            Debug.LogError($"Cannot find DialogueActor with name: \"{line.CharacterName}\"");
            return;
        }

        SpriteRenderer spriteRenderer;
        if (!actor.TryGetComponent<SpriteRenderer>(out spriteRenderer)) {
            Debug.LogError($"Cannot find SpriteRenderer with associated DialogueActor for: \"{line.CharacterName}\"");
            return;
        }

        Vector3 topCenterPoint = spriteRenderer.bounds.center + new Vector3(0, spriteRenderer.bounds.extents.y, 0);
        Vector3 newPivot = _camera.WorldToScreenPoint(topCenterPoint);
        newPivot.z = 0;
        transform.position = newPivot;

        // Parse line text
        MarkupParseResult text = line.TextWithoutCharacterName;

        Typewriter ??= new InstantTypewriter() {
            ActionMarkupHandlers = this.ActionMarkupHandlers,
            Text = this.lineText,
        };

        Typewriter.PrepareForContent(text);

        if (canvasGroup != null) {
            // fading up the UI
            if (useFadeEffect) {
                await Effects.FadeAlphaAsync(canvasGroup, 0, 1, fadeUpDuration, token.HurryUpToken);
            } else {
                // We're not fading up, so set the canvas group's alpha to 1 immediately.
                canvasGroup.alpha = 1;
            }
        }

        await Typewriter.RunTypewriter(text, token.HurryUpToken).SuppressCancellationThrow();

        await YarnTask.Delay((int)(autoAdvanceDelay * 1000), token.NextContentToken).SuppressCancellationThrow();

        Typewriter.ContentWillDismiss();

        if (canvasGroup != null) {
            // we fade down the UI
            if (useFadeEffect) {
                await Effects.FadeAlphaAsync(canvasGroup, 1, 0, fadeDownDuration, token.HurryUpToken).SuppressCancellationThrow();
            } else {
                canvasGroup.alpha = 0;
            }
        }
    }
}
