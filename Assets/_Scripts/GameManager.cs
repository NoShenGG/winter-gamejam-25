using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    public Dictionary<string, DialogueActor> DialogueActors;
    public Camera MainCamera;

    private void Awake() {
        Initialize();
        DialogueActors = new Dictionary<string, DialogueActor>();
    }

    private void Start() {
        MainCamera = Camera.main;
    }
    
    private void Initialize() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}