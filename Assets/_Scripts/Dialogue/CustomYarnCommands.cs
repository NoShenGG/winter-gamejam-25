using UnityEngine;
using Yarn.Unity;

[RequireComponent(typeof(DialogueRunner))]
public class CustomYarnCommands : MonoBehaviour {
    private DialogueRunner _dialogueRunner;

    public void Awake() {
        _dialogueRunner = gameObject.GetComponent<DialogueRunner>();
    }

    private void SetDialogueBubbleType() {

    }
}
