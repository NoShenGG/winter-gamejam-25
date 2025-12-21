using UnityEngine;

public class BoundedCamera : MonoBehaviour
{   
    [SerializeField] private Vector3 camera_offset;
    [SerializeField] private float smooth_speed;
    [SerializeField] private PlayerController player;
    public static BoundedCamera Instance; 
    private Vector3 targetLocation;
    // private Vector3[] bounds = new Vector3[2]; // In TopRight, BottomRight, BottomLeft, TopLeft order. Bounds bind the actual camera location; the camera will just out a bit more.
    private Vector3 bottomLeftBoundary;
    private Vector3 topRightBoundary;
    private bool _InRoomTransition = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(this.gameObject);
        }
        
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_InRoomTransition)
        {
            transform.position = Vector3.Lerp(transform.position, targetLocation, smooth_speed);
            if (Vector3.Distance(transform.position, targetLocation) < 0.01f) {
                _InRoomTransition = false;
                player.ResumeAfterCameraTransition();
            }
        } 
        else
        {
            // Camera follow until bounds
            transform.position = player.gameObject.transform.position + camera_offset;
            Vector3 boundedPosition = transform.position;
            if (transform.position.x < bottomLeftBoundary.x) boundedPosition.x = bottomLeftBoundary.x;
            if (transform.position.y < bottomLeftBoundary.y) boundedPosition.y = bottomLeftBoundary.y;
            if (transform.position.x > topRightBoundary.x) boundedPosition.x = topRightBoundary.x;
            if (transform.position.y > topRightBoundary.y) boundedPosition.y = topRightBoundary.y;
            transform.position = boundedPosition;
        }
    }

    public void TransitionToNextRoom(Vector2 newTargetLocation, Vector3 bottomLeftBound, Vector3 topRightBound)
    {
        targetLocation = new Vector3(newTargetLocation.x, newTargetLocation.y, camera_offset.z);
        bottomLeftBoundary = bottomLeftBound;
        topRightBoundary = topRightBound;
        _InRoomTransition = true;
        player.PauseForCameraTransition();
    }
}
