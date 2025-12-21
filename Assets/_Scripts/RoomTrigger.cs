using UnityEngine;
using Yarn.Unity;

public class RoomTrigger : MonoBehaviour
{
    enum type {
        Death,
        GoNext,
        GoPrevious,
        Cutscene
    }
    [SerializeField] private type _type;
    [SerializeField] private string cutsceneDialogueTitle;
    private bool _cutsceneTriggered = false;
    void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log("Try to reset the room, yipee!");
        if (other.tag.Equals("Player"))
        {
            switch (_type)
            {
                case type.Death:
                    LevelManager.Instance.ResetRoom();
                    break;
                case type.GoNext:
                    LevelManager.Instance.GoToNextRoom();
                    break;
                case type.GoPrevious:
                    LevelManager.Instance.GoToPreviousRoom();
                    break;
                case type.Cutscene:
                    if (!_cutsceneTriggered) {
                        DialogueRunner dialogue = GameManager.Instance.DialogueRunner;
                        dialogue.StartDialogue(cutsceneDialogueTitle);
                        GameManager.Instance.PlayerController.DisableInput();
                        dialogue.onDialogueComplete.AddListener(OnDialogueComplete);
                    }
                    break;
                default:
                    break;
            }
           
            // LevelManager.Instance.ResetRoom();
            // LevelManager.Instance.GoToNextRoom();
        }

    }

    void OnDialogueComplete() {
        _cutsceneTriggered = true;
        GameManager.Instance.PlayerController.EnableInput();
    }
}
