using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CanvasGroup))]
public class CameraOverlay : MonoBehaviour {
    public CanvasGroup CanvasGroup;
    private Camera _camera;

    private void Start() {
        GameManager.Instance.CameraUIOverlay = this;
        CanvasGroup = GetComponent<CanvasGroup>();
        _camera = Camera.main;
    }

    private void Update() {
        transform.position = Mouse.current.position.ReadValue();
    }

    public List<GameObject> FindOverlappingObjects(List<GameObject> visibles) {
        List<GameObject> result = new List<GameObject>();

        foreach (GameObject obj in visibles) {
            Vector2 position = _camera.WorldToScreenPoint(obj.transform.position);
            if (RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), position)) {
                result.Add(obj);
            }
        }

        return result;
    }
}
