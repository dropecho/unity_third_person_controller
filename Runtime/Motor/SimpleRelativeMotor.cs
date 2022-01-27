using Dropecho;
using UnityEngine;

namespace Dropecho {
  public class SimpleRelativeMotor : MonoBehaviour, ICharacterMotor {
    [Tooltip("The speed at which the object moves forward.")]
    public float moveSpeed = 5f;
    [Tooltip("The speed at which the object turns.")]
    public float turnSpeed = 360;

    Camera _camera;

    void OnEnable() => _camera = Camera.main;

    public void Move(Vector2 input, float forwardModifier = 1) {
      if (input == Vector2.zero) return;

      var move = Vector3.ProjectOnPlane(GetCameraRelativeInput(input), Vector3.up);
      var turnAmount = Mathf.Clamp(Mathf.Atan2(move.x, move.z), -1, 1);
      var fwd = move.z * forwardModifier;

      transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
      transform.position += transform.forward * fwd * moveSpeed * Time.deltaTime;
    }

    Vector3 GetCameraRelativeInput(Vector2 input) {
      var camFwd = Vector3.ProjectOnPlane(_camera.transform.forward, Vector3.up).normalized;
      var rotationToCamera = Quaternion.FromToRotation(transform.forward, camFwd);
      return rotationToCamera * new Vector3(input.x, 0, input.y);
    }
  }
}