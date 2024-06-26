using UnityEngine;

public class CameraOperator : MonoBehaviour {
    private Camera cam;
    public float speed = 2.0f;
    public Vector2 xBounds = new Vector2(7, 100);
    public Vector2 yBounds = new Vector2(3, 100);

    private void Start() {
        cam = Camera.main;
    }

    private void Update() {
        if (Input.GetKey(KeyCode.W)) {
            cam.transform.position += Vector3.up * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A)) {
            cam.transform.position += Vector3.left * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S)) {
            cam.transform.position += Vector3.down * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D)) {
            cam.transform.position += Vector3.right * speed * Time.deltaTime;
        }

        cam.transform.position = new Vector3(
            Mathf.Clamp(cam.transform.position.x, xBounds.x, xBounds.y),
            Mathf.Clamp(cam.transform.position.y, yBounds.x, yBounds.y),
            cam.transform.position.z
        );
    }
}