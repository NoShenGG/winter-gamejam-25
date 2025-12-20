using UnityEngine;
using Yarn.Unity;

public class CustomYarnCommands : MonoBehaviour {
    [SerializeField]
    private DialogueRunner dialogueRunner;

    public void Awake() {
        if (dialogueRunner != null) {
            // Empty for future implementation
        } else {
            Debug.LogError("Dialogue Runner not set in CustomYarnCommands");
        }
    }
}
