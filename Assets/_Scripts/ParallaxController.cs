using UnityEngine;

public class ParallaxController : MonoBehaviour {
    private float _startPos;
    private Camera _camera;
    public float ParallaxMultiplier;

    private void Start() {
        _startPos = transform.position.x;
        _camera = Camera.main;
    }

    private void Update() {
        float parallaxDist = _camera.transform.position.x * ParallaxMultiplier;

        transform.position = new Vector3(_startPos + parallaxDist, transform.position.y, transform.position.z);
    }
}
