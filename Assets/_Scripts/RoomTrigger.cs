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
    [SerializeField] private DialogueRunner cutsceneDialogue;
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
                    break;
                case type.Cutscene:
                    if (!_cutsceneTriggered) {
                        cutsceneDialogue.StartDialogue(cutsceneDialogueTitle);
                    }
                    break;
                default:
                    break;
            }
           
            // LevelManager.Instance.ResetRoom();
            // LevelManager.Instance.GoToNextRoom();
        }

    }
}
