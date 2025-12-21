using UnityEngine;

public class ParallaxController : MonoBehaviour {
    private Vector2 _startPos;
    private Camera _camera;
    public float ParallaxMultiplier;

    private void Start() {
        _startPos = transform.position;
        _camera = Camera.main;
    }

    private void Update() {
        float parallaxDist = _camera.transform.position.x * ParallaxMultiplier;

        transform.position = new Vector3(_startPos.x + parallaxDist, _startPos.y + _camera.transform.position.y, transform.position.z);
    }
}
