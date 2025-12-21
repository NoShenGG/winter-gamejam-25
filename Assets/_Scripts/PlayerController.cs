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

    // Hidden backend Player Parameters
    private float coyote_timer;
    private Vector2 playerSize;
    private Rigidbody2D rb;
    private CapsuleCollider2D boxCollider;
    private float detectionRadius = 0.03f;
    private Vector2 playerDirection;
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

    bool IsNextToWall()
    {
        return Physics2D.Raycast(playerTransform, playerDirection, detectionRadius);
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
        

        CheckGrab();

        float wall_grab_modifier = 1.0f;

        if (_isNextToWall) {
            wall_grab_modifier = 0.2f;   
        }
        
        rb.linearVelocityX = playerInput.x * speed;
        rb.linearVelocityY = Mathf.Max(rb.linearVelocityY - fall_acceleration * wall_grab_modifier, max_fall_speed);
        if (_isGrabbing) rb.linearVelocityY = playerInput.y * climb_speed; 
    }

    public void CheckGrab()
    {

        float xInput = _moveAction.ReadValue<Vector2>().x;
        // Do a BoxCast in the direction of the player's input
        RaycastHit2D hit = Physics2D.Raycast(playerTransform, Vector2.right * xInput, (playerSize.x / 2) + detectionRadius * 2, groundLayer);
        
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
