using UnityEngine;
 
public class CameraController : MonoBehaviour {
    public float moveSpeed = 1f;
    public AnimationCurve curve;
    private Vector3 dragOrigin;
    private Vector3 prevPosition;
    private Vector3 screenpoint;
    private Vector3 velocity;
    private Vector3 inertialVelocity;
    private bool underIntertia;
    private float time = 0.0f;
    private float SmoothTime = 1000;

    void Update() {
      if (Input.GetMouseButtonDown(0)) {
          screenpoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5);
          dragOrigin = Camera.main.ScreenToWorldPoint(screenpoint);
          underIntertia = false;
          return;
      }
      if (Input.GetMouseButtonUp(0)) {
        Vector3 currentScreenpoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5);
        inertialVelocity = dragOrigin - Camera.main.ScreenToWorldPoint(currentScreenpoint);
        underIntertia = true;
        return;
      }
      if(underIntertia && time <= SmoothTime) {
        velocity = Vector3.Lerp(inertialVelocity, Vector3.zero, curve.Evaluate(time));
        transform.Translate(velocity);
        transform.position = clampedPosition();
        time += Time.smoothDeltaTime;
      } else if (!Input.GetMouseButton(0)) {
          return;
      } else {
          Vector3 currentScreenpoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5);
          velocity = dragOrigin - Camera.main.ScreenToWorldPoint(currentScreenpoint);
          transform.Translate(velocity, Space.World);
          transform.position = clampedPosition();
          underIntertia = false;
          time = 0;
      }
    }

    Vector3 clampedPosition() {
      Vector3 v = new Vector3(
          Mathf.Clamp(transform.position.x, 0, 45),
          Mathf.Clamp(transform.position.y, 0, 35),
          transform.position.z
        );
      return v;
    }
}
