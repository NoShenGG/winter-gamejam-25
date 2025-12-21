using UnityEngine;

public class ParallaxController : MonoBehaviour {
    private float _startPos;
    public float ParallaxMultiplier;

    private void Start() {
        _startPos = transform.position.x;
    }

    private void Update() {
        Camera camera = GameManager.Instance.MainCamera;
        float parallaxDist = camera.transform.position.x * ParallaxMultiplier;

        transform.position = new Vector3(_startPos + parallaxDist, transform.position.y, transform.position.z);
    }
}
