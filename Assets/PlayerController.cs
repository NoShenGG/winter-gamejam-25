using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private float jump_speed;
    [SerializeField] private float fall_acceleration;
    [SerializeField] private float max_coyote_time;
    private float coyote_timer;
    private Vector2 playerSize;
    Rigidbody2D rb;
    private float detectionRadius = 0.25f;
    private Vector2 playerDirection;
    private bool _wasGrounded = false;

    Vector2 playerTransform
    {
        get
        {
            return new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
        }
    }
    private bool IsGrounded
    {
        get
        {
            // Checks detection for if the player is near the ground
            LayerMask groundLayer = LayerMask.GetMask("Ground");

            bool player_is_touching_ground = Physics2D.BoxCast(playerTransform, playerSize, 0, -Vector2.up, (rb.transform.localScale.y / 2f) + detectionRadius, groundLayer);

            // If the player is touching the ground, no issue
            if (player_is_touching_ground) 
            {
                Debug.Log("player is touching ground");
                _wasGrounded = true;
                return true;
            } 
            // Else, if the player was touching the ground previously, start the Coyote Timer
            // Once the coyote timer has counted down, then the player is no longer touching the ground.
            else
            {
                if (_wasGrounded)
                {
                    _wasGrounded = false;
                    coyote_timer = max_coyote_time;
                    Debug.Log("Just set the coyote timer!");
                    return true; 
                }
                if (coyote_timer == 0f)
                {
                    Debug.Log("The player is not grounded");
                    return false;
                }
                return true;
            }
        }
        set
        {
            _wasGrounded = value;
        }
    }

    bool IsNextToWall()
    {
        return Physics2D.Raycast(playerTransform, playerDirection, detectionRadius);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        playerSize = new Vector2(transform.localScale.x, transform.localScale.y);
    }

    // Update is called once per frame
    void Update()
    {
        // If the coyote_timer is active, then update the coyote timer
        if (coyote_timer > 0f)
        {
            coyote_timer = Mathf.Max(coyote_timer - Time.deltaTime, 0f);
        }

        // If the player is not grounded, accelerate them until max fall speed
        // Debug.Log("Checked if is grounded");
        if (!IsGrounded)
        {
            // Debug.Log("Is not grounded, engage safety protocols");
            float max_fall_speed = -2;
            rb.linearVelocityY = Mathf.Max(rb.linearVelocityY - fall_acceleration, max_fall_speed);
        }
    }

    public void OnMove(InputValue value)
    {
        // Debug.Log("On move called");
        rb.linearVelocityX = value.Get<Vector2>().x * speed;
    }

    public void OnJump(InputValue value)
    {
        if (IsGrounded)
        {
            rb.linearVelocityY = jump_speed;
            // Done to avoid coyote time extending the jump. Otherwise entirely unecessary
            IsGrounded = false;
            coyote_timer = 0f;
        }
        // Debug.Log($"Tried to jump!, is grounded status is {IsGrounded}");
    }

    public void OnGrab(InputValue value)
    {
        
    }

}
