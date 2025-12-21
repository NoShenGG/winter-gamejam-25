using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class GameManager : MonoBehaviour {
    #region Singleton

    private static GameManager _instance;
    public static GameManager Instance {
        get {
            if (_instance == null) {
                GameObject gameManager = new GameObject("GameManager");
                _instance = gameManager.AddComponent<GameManager>();
            }
            return _instance;
        }
        private set {
            _instance = value;
        }
    }

    #endregion

    public Dictionary<string, DialogueActor> DialogueActors;
    public PlayerController PlayerController;
    public DialogueRunner DialogueRunner;
    public List<GameObject> VisibleObjects;

    private void Awake() {
        Initialize();
        DialogueActors = new Dictionary<string, DialogueActor>();
    }

    private void Initialize() {
        if (_instance == null) {
            _instance = this;
        } else if (_instance != this) {
            Destroy(this);
        }
    }
}