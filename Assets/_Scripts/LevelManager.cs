using UnityEngine;
using System.Collections.Generic;
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance
    {
        get; private set;
    }

    public Room[] rooms;
    private int currentRoomIndex = 0;
    private GameObject player;


    
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(this.gameObject);
        }
        player = GameObject.Find("Player").GetComponentInChildren<PlayerController>().gameObject; // Sequenced to get Game Object with PlayerControler component
        InstantiateRoomArray();
        LoadRoom(0);
    }

    private void InstantiateRoomArray()
    {
        rooms = Instance.GetComponentsInChildren<Room>();
        for (int i = 0; i < rooms.Length; i++)
        {
            rooms[i].InitializeRoom();
        }
    }

    public void ResetRoom()
    {
        LoadRoom(currentRoomIndex);
        player.transform.position = rooms[currentRoomIndex].RespawnPoint;
    }

    public void GoToNextRoom()
    {
        LoadRoom(currentRoomIndex + 1);
        // player.transform.position = rooms[currentRoomIndex].RespawnPoint;
    }
    
    private void LoadRoom(int index)
    {
        if (index >= rooms.Length || index < 0)
        {
            Debug.LogError($"Tried to load a room of index {index}, but it was out of bounds!");
            return;
        }
        rooms[index].ActivateRoom();
        currentRoomIndex = index;
        Vector2 targetPosition = new Vector3(rooms[index].CameraPosition.x, rooms[index].CameraPosition.y, -10);
        Vector2[] bounds = rooms[index].GetBounds(); // Gets bounds as bottom_left, top_right
        BoundedCamera.Instance.TransitionToNextRoom(targetPosition, bounds[0], bounds[1]);
    }
    
    private void UnloadRoom(int index)
    {
        if (index >= rooms.Length || index < 0)
        {
            Debug.LogError($"Tried to unload a room of index {index}, but it was out of bounds!");
            return;
        }
        rooms[index].DeactivateRoom();
    }
    
}
