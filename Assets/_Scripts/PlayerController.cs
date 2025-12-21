using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Setting up Player Controls
    [SerializeField] private InputActionAsset inputActions;
    private InputActionMap _playerControls;
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _grabAction;

    // Player Parameters that can be modified
    [Header("Player Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float climb_speed;
    [SerializeField] private float jump_speed;
    [SerializeField] private float fall_acceleration;
    [SerializeField] private float max_coyote_time;

    // Hidden backend Player Parameters
    private float coyote_timer;
    private Vector2 playerSize;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private float detectionRadius = 0.05f;
    private Vector2 playerDirection;
    private LayerMask groundLayer;
    private bool _isGrabbing;
    private bool _isNextToWall;
    
    Vector2 playerTransform
    {
        get
        {
            return new Vector2(transform.position.x, transform.position.y);
        }
    }
    private bool CanJump
    {
        get
        {
            if (coyote_timer > 0f)
            {
                return true;
            }
            return false;
        }
    }

    bool IsNextToWall()
    {
        return Physics2D.Raycast(playerTransform, playerDirection, detectionRadius);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetControls();
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        playerSize = boxCollider.size - new Vector2(0.05f, 0.05f);
        groundLayer = LayerMask.GetMask("Ground");
    }

    private void SetControls()
    {
        _playerControls = inputActions.FindActionMap("Player");

        _moveAction = _playerControls.FindAction("Move");
        _jumpAction = _playerControls.FindAction("Jump");
        _grabAction = _playerControls.FindAction("Grab");

        _playerControls.Enable();
        _moveAction.started += OnMovePressed;
        _moveAction.canceled += OnMoveReleased;
        _jumpAction.started += OnJumpPressed;
        _grabAction.started += OnGrabPressed;
        _grabAction.canceled += OnGrabReleased;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool player_is_touching_ground = Physics2D.BoxCast(playerTransform, playerSize, 0, Vector2.down, detectionRadius, groundLayer);
        // Debug.DrawLine(transform.position, transform.position + new Vector3(detectionRadius * 2 + (playerSize.x / 2), 0, 0));
        // Debug.DrawLine(transform.position, transform.position - new Vector3(0, (rb.transform.localScale.y / 2f) + detectionRadius, 0));
        // Something about this is broken; I'm attemping to set the coyote timer up only if the player is touching the ground, and set it to 0 such that the player
        // can't jump after jumping. This isn't working.
        if (player_is_touching_ground)
        {
            // Debug.Log("Player is touching ground right now");
            coyote_timer = max_coyote_time;
        } 
        else
        {
            coyote_timer = Mathf.Max(coyote_timer - Time.deltaTime, 0f);
        }

        CheckGrab();

        // We need to reset the fall speed once hitting a wall.
        // Kill the momentum. Maybe something along the lines of _was_grabbbing?
        // If the player is not grounded, accelerate them until max fall speed
        // Also if the player is not grabbing, let gravity apply
        float wall_grab_modifier = 1.0f;

        if (_isNextToWall) {
            if (_isGrabbing) wall_grab_modifier = 0f;
            else wall_grab_modifier = 0.2f;
        }
        
        // Debug.Log("Is not grounded, engage safety protocols");
        float max_fall_speed = -2;
        rb.linearVelocityY = Mathf.Max(rb.linearVelocityY - fall_acceleration * wall_grab_modifier, max_fall_speed);
    }

    // This is done through axis, and this registers two inputs - when a button has been pressed and when it's been released
    public void OnMovePressed(InputAction.CallbackContext context)
    {
        // Debug.Log("On move called");
        float xInput = context.ReadValue<Vector2>().x;
        rb.linearVelocityX = xInput * speed;
        if (_isGrabbing && _isNextToWall)
        {
            float yInput = context.ReadValue<Vector2>().y;
            rb.linearVelocityY = yInput * climb_speed;
        }
        
    }

    public void CheckGrab()
    {

        float xInput = _moveAction.ReadValue<Vector2>().x;
        // Debug.Log(Vector2.right * Mathf.Sign(xInput));
        // Do a BoxCast in the direction of the player's input
        RaycastHit2D hit = Physics2D.Raycast(playerTransform, Vector2.right * xInput, (playerSize.x / 2) + detectionRadius * 2, groundLayer);
        
        // If this would cause a grab, then do something about it
        if (hit)
        {
            // Debug.Log($"Hey, we detected somethiing! {hit.collider.gameObject.name}");
            // Slight bug with collision, freeze xVelocity
            if (hit.collider.GetComponent<Grabbable>())
            {
                if (!_isNextToWall)
                {
                    rb.linearVelocityY = 0;
                    rb.linearVelocityX = 0;
                    _isNextToWall = true;
                }
                // Add some code about right vs. left facing grabs here.
                // Debug.Log("We're grabbing a wall!");
            }            
        } else
        {
            _isNextToWall = false;
        }
        
        // Debug.Log("Performing!");
    }

    public void OnMoveReleased(InputAction.CallbackContext context)
    {

        rb.linearVelocityX = 0;
        // if (_isNextToWall)
        // {
            // rb.linearVelocityY = 0;
            // _isNextToWall = false;
        // }
        // Do a BoxCast in the direction of the player's input
        // RaycastHit2D hit = Physics2D.Raycast(playerTransform, Vector2.right * Mathf.Sign(xInput), (rb.transform.localScale.y / 2f) + detectionRadius, groundLayer);
        // // If this would cause a grab, then do something about it
        // if (hit)
        // {
        //     if (hit.collider.GetComponent<Grabbable>())
        //     {
        //         Debug.Log("We're grabbing a wall!");
        //     }            
        // }
    }


    public void OnJumpPressed(InputAction.CallbackContext context)
    {
        if (CanJump)
        {
            rb.linearVelocityY = jump_speed;
            // Done to avoid coyote time extending the jump. Otherwise entirely unecessary
            coyote_timer = 0f;
        }
    }

    public void OnGrabPressed(InputAction.CallbackContext context)
    {
        _isGrabbing = true;
        if (_isNextToWall)
        {
            rb.linearVelocityY = 0;
        }
    }

    public void OnGrabReleased(InputAction.CallbackContext context)
    {
        _isGrabbing = false;
    }

}
