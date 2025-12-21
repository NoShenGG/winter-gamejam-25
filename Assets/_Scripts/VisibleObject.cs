using System.Collections.Generic;
using UnityEngine;

public class VisibleObject : MonoBehaviour {
    private List<GameObject> visibles;

    private void Start() {
        visibles = GameManager.Instance.VisibleObjects;
    }

    private void OnBecameVisible() {
        if (!visibles.Contains(this.gameObject)) {
            visibles.Add(this.gameObject);
        }
    }

    private void OnBecameInvisible() {
        if (visibles.Contains(this.gameObject)) {
            visibles.Remove(this.gameObject);
        }
    }
}
