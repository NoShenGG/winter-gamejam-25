using UnityEngine;

public class Boxable : MonoBehaviour
{
    [SerializeField] private Vector2 held_offset;
    private bool _grabbed;
    private Rigidbody2D rb;
    [SerializeField] private float fall_acceleration = 0.2f;
    [SerializeField] private float max_fall_speed = -4f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_grabbed)
        {
            Vector2 box_position = held_offset * LevelManager.Instance.Player.playerDirection;
            transform.position = LevelManager.Instance.Player.gameObject.transform.position + new Vector3(box_position.x, box_position.y, 0);
        } else
        {
            rb.linearVelocityY = Mathf.Max(rb.linearVelocityY - fall_acceleration, max_fall_speed);
        }
    }

    public void Grab()
    {
        _grabbed = true;
    }

    public void Release()
    {
        _grabbed = false;
    }
}
