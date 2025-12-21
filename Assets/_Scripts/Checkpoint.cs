using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    [SerializeField] private Room room;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        room.updateRespawn(this.transform.position.x, this.transform.position.y);
    }
}
