using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Transform target;
    public float smoothing = 5f;

    Vector3 offset;

    void Start() {
        offset = new Vector3(-6, 10, -6);
    }

    void FixedUpdate() {
        Vector3 targetCameraPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetCameraPos, smoothing * Time.deltaTime);
    }
}