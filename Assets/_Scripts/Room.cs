using UnityEngine;
using System.Collections.Generic;

public class Room : MonoBehaviour
{
    
    [SerializeField] private Vector2 respawn_point;
    [SerializeField] private Vector2 bottom_left_bound;
    [SerializeField] private Vector2 top_right_bound;
    

    public Vector2 RespawnPoint
    {
        // Stored until used by the Level Manager for resetting the player.
        get
        {
            return respawn_point;
        }
    }              
    [SerializeField] private Vector2 camera_position;
    public Vector2 CameraPosition
    {
        get
        {
            return camera_position;
        }
    }

    // Backend regarding where each object is reset to.
    List<GameObject> objects_in_room = new List<GameObject>();
    List<Vector3> stored_positions = new List<Vector3>(); 


    public void InitializeRoom()
    {
        foreach (Transform child in transform)
        {
            objects_in_room.Add(child.gameObject);
            stored_positions.Add(child.position);
        } 
    }

    // Function should also be used when reseting the room
    public void ActivateRoom()
    {
        for (int i = 0; i < objects_in_room.Count; i++)
        {
            objects_in_room[i].transform.position = stored_positions[i];
            objects_in_room[i].SetActive(true);
        }
    }

    // I love Occlusion Yipee
    public void DeactivateRoom()
    {
        foreach (GameObject obj in objects_in_room)
        {
            obj.SetActive(false);
        }
    }

    public Vector2[] GetBounds()
    {
        Vector2[] bounds = {bottom_left_bound, top_right_bound}; 
        return bounds;
    }
}
