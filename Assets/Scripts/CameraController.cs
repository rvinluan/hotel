using UnityEngine;
 
public class CameraController : MonoBehaviour {
    public float moveSpeed = 1f;
    public AnimationCurve curve;
    private Vector3 dragOrigin;
    private Vector3 prevPosition;
    private Vector3 screenpoint;
    private Vector3 velocity;
    private bool underIntertia;
    private float time = 0.0f;
    private float SmoothTime = 3000;

    void Update() {
      if (Input.GetMouseButtonDown(0)) {
          screenpoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5);
          dragOrigin = Camera.main.ScreenToWorldPoint(screenpoint);
          underIntertia = false;
          return;
      }
      if (Input.GetMouseButtonUp(0)) {
        underIntertia = true;
        return;
      }
      if(underIntertia && time <= SmoothTime) {
        transform.Translate(velocity);
        velocity = Vector3.Lerp(velocity, Vector3.zero, curve.Evaluate(time));
        time += Time.smoothDeltaTime;
      } else if (!Input.GetMouseButton(0)) {
          return;
      } else {
          Vector3 currentScreenpoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5);
          velocity = dragOrigin - Camera.main.ScreenToWorldPoint(currentScreenpoint);
          transform.Translate(velocity, Space.World);
          underIntertia = false;
          time = 0;
      }
    }
}
