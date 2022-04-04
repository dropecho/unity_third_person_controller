using UnityEngine;

namespace Dropecho {
  [RequireComponent(typeof(Animator))]
  public class RootMotionMotor : MonoBehaviour, ICharacterMotor {
    public float stationaryTurnSpeed = 180;
    public float movingTurnSpeed = 360;

    Animator _animator;
    void Awake() => _animator = GetComponent<Animator>();

    public void Move(Vector2 input, float forwardModifier = 1) {
      var move = transform.InverseTransformDirection(new Vector3(input.x, 0, input.y));
      var turnAmount = Mathf.Clamp(Mathf.Atan2(move.x, move.z), -1, 1);

      ApplyAdditionalRotation(move.z, turnAmount);

      _animator.SetFloat("horizontal", turnAmount, 0.1f, Time.deltaTime);
      _animator.SetFloat("forward", move.z * forwardModifier, 0.1f, Time.deltaTime);
    }

    void ApplyAdditionalRotation(float fwd, float turnAmount) {
      var turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, fwd);
      transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
    }

    void OnAnimatorMove() {
      if (Time.deltaTime > 0) {
        var delta = _animator.deltaPosition;
        delta.y = 0;
        transform.position += delta;
      }
    }
  }
}