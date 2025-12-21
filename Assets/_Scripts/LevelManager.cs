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
    public PlayerController Player
    {
        get
        {
            return player.gameObject.GetComponent<PlayerController>();
        }
    }


    
    void Awake()
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
    }

    void Start()
    {
        LoadRoom(0);
        for (int i = 1; i < rooms.Length; i++)
        {
            UnloadRoom(i);
        }
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
        UnloadRoom(currentRoomIndex);
        LoadRoom(currentRoomIndex + 1);
        
        // player.transform.position = rooms[currentRoomIndex].RespawnPoint;
    }

    public void GoToPreviousRoom()
    {
        UnloadRoom(currentRoomIndex);
        LoadRoom(currentRoomIndex - 1);
        Vector2[] bounds = rooms[currentRoomIndex].GetBounds(); // Gets bounds as bottom_left, top_right
        BoundedCamera.Instance.TransitionToNextRoom(bounds[1], bounds[0], bounds[1]); 
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

        Vector2 targetPosition = new Vector3(rooms[currentRoomIndex].CameraPosition.x, rooms[currentRoomIndex].CameraPosition.y, -10);
        Vector2[] bounds = rooms[currentRoomIndex].GetBounds(); // Gets bounds as bottom_left, top_right
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
    
    public Room GetRoom() {
        return rooms[currentRoomIndex];
    }
}
