using UnityEngine;
 
public class CameraController : MonoBehaviour {
    public float moveSpeed = 1f;
    private Vector3 dragOrigin;

    void Update() {
      if (Input.GetMouseButtonDown(0)) {
          dragOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
          return;
      }
      if (!Input.GetMouseButton(0)) {
          return;
      }

      Vector3 delta = dragOrigin - Camera.main.ScreenToWorldPoint(Input.mousePosition);
      transform.Translate(delta * moveSpeed, Space.World);
    }
}
