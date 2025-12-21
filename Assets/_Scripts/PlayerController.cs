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
    [SerializeField] private float max_fall_speed;
    [SerializeField] private float max_coyote_time;

    [SerializeField] private Animator animator;

    // Hidden backend Player Parameters
    private float coyote_timer;
    private Vector2 playerSize;
    private Rigidbody2D rb;
    private CapsuleCollider2D boxCollider;
    private float detectionRadius = 0.03f;
    private float playerDirection;
    private LayerMask groundLayer;
    private bool _isGrabbing;
    private bool _isNextToWall;

    private Vector2 former_speed; // Speed I need this (for when camera transition)
    
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetControls();
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<CapsuleCollider2D>();
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
        // _moveAction.started += OnMovePressed;
        // _moveAction.canceled += OnMoveReleased;
        _grabAction.canceled += OnGrabReleased;
        _jumpAction.started += OnJumpPressed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_playerControls.enabled) return;
        bool player_is_touching_ground = Physics2D.BoxCast(playerTransform, playerSize, 0, Vector2.down, detectionRadius, groundLayer);
        if (player_is_touching_ground)
        {
            // Debug.Log("Player is touching ground right now");
            coyote_timer = max_coyote_time;
        } 
        else
        {
            coyote_timer = Mathf.Max(coyote_timer - Time.deltaTime, 0f);
        }

        // Read the input of the player for movement
        Vector2 playerInput = _moveAction.ReadValue<Vector2>();
        
        CheckNextToWall();
        CheckGrab();

        float wall_grab_modifier = 1.0f;

        if (_isNextToWall) {
            wall_grab_modifier = 0.2f;   
        }
        
        if (!_isGrabbing) rb.linearVelocityX = playerInput.x * speed;
        rb.linearVelocityY = Mathf.Max(rb.linearVelocityY - fall_acceleration * wall_grab_modifier, max_fall_speed);
        if (_isGrabbing) {
            rb.linearVelocityY = playerInput.y * climb_speed;
            rb.linearVelocityX = 0;
        }

        animator.SetFloat("Vel_H", rb.linearVelocityX);
        animator.SetBool("Grounded", player_is_touching_ground);
    }

    public void CheckNextToWall()
    {
        // Check the left and right of the player.
        // This implementation is shoddy in that
        // There's probably something better by just using the current player movement input to check for a raycast hit, 
        // And then preserving that information for later. If future me takes a look at this, I applaud you.
        RaycastHit2D rightHit = Physics2D.Raycast(playerTransform, Vector2.right, (playerSize.x / 2) + detectionRadius * 2, groundLayer);
        if (rightHit) {
            playerDirection = 1f;
        }
        RaycastHit2D leftHit = Physics2D.Raycast(playerTransform, Vector2.left, (playerSize.x / 2) + detectionRadius * 2, groundLayer);
        if (leftHit)
        {
            playerDirection = -1f;
        }

        // Preserve the current grab direction to prevent the player from floating off into oblivion
        
        // Not sure if this works honestly, but the intention is that if right hit is true use that, else use left hit
        // And if left hit also isn't true it shouldn't pass the hit check
        RaycastHit2D hit = rightHit? rightHit : leftHit;
        // If the player is next to the wall, next check for grab
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
                
                _isGrabbing = _grabAction.ReadValue<float>() == 1;
            }            
        } else
        {
            _isNextToWall = false;
        }
    }

    public void CheckGrab()
    {
        if (_isGrabbing)
        {
            RaycastHit2D hit = Physics2D.Raycast(playerTransform, Vector2.right * playerDirection, (playerSize.x / 2) + detectionRadius * 2, groundLayer);
            if (!hit)
            {
                _isGrabbing = false;
            }
        }
        
        
        // Debug.Log("Performing!");
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

    public void OnGrabReleased(InputAction.CallbackContext context)
    {
        _isGrabbing = false;
    }

    public void PauseForCameraTransition()
    {
        former_speed = rb.linearVelocity;
        rb.linearVelocity = new Vector2(0, 0);
        _playerControls.Disable();
    }

    public void ResumeAfterCameraTransition()
    {
        rb.linearVelocity = former_speed;
        _playerControls.Enable();
    }
}
