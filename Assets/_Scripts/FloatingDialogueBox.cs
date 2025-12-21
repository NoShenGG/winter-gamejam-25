using UnityEngine;
using TMPro;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class FloatingDialogueBox : MonoBehaviour {
    public TMP_Text content;
    public LayoutElement layoutElement;
    
    private void Update() {
        layoutElement.enabled = content.preferredWidth >= layoutElement.preferredWidth;
    }
}
