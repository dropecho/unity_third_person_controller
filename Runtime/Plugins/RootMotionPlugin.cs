using UnityEngine;
#if ENABLE_INPUT_SYSTEM
  using UnityEngine.InputSystem;
#endif

namespace Dropecho {
  public class RootMotionPlugin : MonoBehaviour, ICharacterMotorPlugin {
    public float moveSpeed = 5;
    public float stationaryTurnSpeed = 180;
    public float movingTurnSpeed = 360;
    public bool freezeRootY = false;
    public bool useRootMotion = true;

    Animator _animator;
    IInputSource _inputSource;
    Vector3 translation;
    private Quaternion rotation;

    void OnEnable() {
      _animator = GetComponentInChildren<Animator>();
      _inputSource = GetComponent<IInputSource>();
    }

    void OnAnimatorMove() {
      if (!useRootMotion) {
        return;
      }

      if (Time.deltaTime > 0) {
        var positionDelta = _animator.deltaPosition;
        if (freezeRootY) {
          positionDelta.y = 0;
        }
        translation = positionDelta;
      }
    }

    void Update() {
      var input = _inputSource.GetInput();
      var move = transform.InverseTransformDirection(new Vector3(input.x, 0, input.y));
      var turnAmount = Mathf.Clamp(Mathf.Atan2(move.x, move.z), -1, 1);

      _animator.SetFloat("horizontal", turnAmount, 0.1f, Time.deltaTime);
      _animator.SetFloat("forward", move.z, 0.1f, Time.deltaTime);

      ApplyAdditionalRotation(move.z, turnAmount);
      if (!useRootMotion) {
        translation = moveSpeed * Time.deltaTime * new Vector3(input.x, 0, input.y);
      }
    }

    void ApplyAdditionalRotation(float fwd, float turnAmount) {
      var turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, fwd);
      rotation = Quaternion.Euler(0, turnAmount * Time.deltaTime * turnSpeed, 0);
    }

    public Vector3 GetTranslation(float delta) => this.translation;

    public void GetMovement(float delta, out Vector3 translation, out Quaternion rotation) {
      rotation = this.rotation;
      translation = GetTranslation(delta);
    }
  }
}