using UnityEngine;

public class CameraScript : MonoBehaviour {
    // Camera variables
    public float CameraDistance = 5.0f;
    public float Sensitivity = 8;
    private float Pitch = 45;
    private float Yaw = -45;

    void Update() {
        if (Input.GetMouseButton(0)) {
            Yaw += Sensitivity * Input.GetAxis("Mouse X");
            Pitch -= Sensitivity * Input.GetAxis("Mouse Y");
            if (Pitch > 90)
                Pitch = 90;
            if (Pitch < -90)
                Pitch = -90;

            // Orbit the camera around the origin.
            gameObject.transform.position = Vector3.back * CameraDistance;
            gameObject.transform.eulerAngles = Vector3.zero;
            gameObject.transform.RotateAround(Vector3.zero, gameObject.transform.up, Yaw);
            gameObject.transform.RotateAround(Vector3.zero, gameObject.transform.right, Pitch);
        }
    }
}
