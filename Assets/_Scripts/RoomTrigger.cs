using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    enum type {
        Death,
        GoNext,
        GoPrevious
    }
    [SerializeField] private type _type;
    void OnTriggerEnter2D(Collider2D other)
    {
         Debug.Log("Try to reset the room, yipee!");
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
                default:
                    break;
            }
           
            // LevelManager.Instance.ResetRoom();
            // LevelManager.Instance.GoToNextRoom();
        }

    }
}
