using UnityEngine;
#if ENABLE_INPUT_SYSTEM
  using UnityEngine.InputSystem;
#endif

namespace Dropecho {
  public class LocomotionPlugin : MonoBehaviour, ICharacterMotorPlugin {
    public float moveSpeed = 4f;
    public float turnSpeed = 200f;
    public bool horizontalRotateOnly = false;
    private IInputSource _inputSource;

    void OnEnable() => _inputSource = GetComponentInParent<IInputSource>();

    public void GetMovement(float delta, out Vector3 translation, out Quaternion rotation) {
      translation = GetTranslation(delta);
      rotation = GetRotation(delta);
    }

    public Vector3 GetTranslation(float delta) {
      var input = _inputSource.GetInput();

      if (horizontalRotateOnly) {
        input *= Vector3.Dot(new Vector3(input.x, 0, input.y), transform.forward);
      }

      return new Vector3(input.x, 0, input.y) * moveSpeed * delta;
    }

    private Quaternion GetRotation(float delta) {
      var input = _inputSource.GetInput();
      var move = transform.InverseTransformDirection(new Vector3(input.x, 0, input.y));
      var turnAmount = Mathf.Clamp(Mathf.Atan2(move.x, move.z), -1, 1);
      return Quaternion.Euler(0, turnAmount * delta * turnSpeed, 0);
    }
  }
}