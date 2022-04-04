using UnityEngine;

namespace Dropecho {
  [RequireComponent(typeof(Animator))]
  public class RootMotionMotor : MonoBehaviour, ICharacterMotor {
    public bool useRootMotion = true;
    public float moveSpeed = 5;
    public float stationaryTurnSpeed = 180;
    public float movingTurnSpeed = 360;

    Animator _animator;
    ICharacterMotorPlugin[] _plugins;

    void OnEnable() {
      _animator = GetComponent<Animator>();
      _plugins = GetComponentsInChildren<ICharacterMotorPlugin>();
    }

    public void Move(Vector2 input, float forwardModifier = 1) {
      var move = transform.InverseTransformDirection(new Vector3(input.x, 0, input.y));
      var turnAmount = Mathf.Clamp(Mathf.Atan2(move.x, move.z), -1, 1);

      ApplyAdditionalRotation(move.z, turnAmount);

      if (!useRootMotion) {
        transform.position += moveSpeed * forwardModifier * Time.deltaTime * new Vector3(input.x, 0, input.y);
        _animator.SetFloat("horizontal", turnAmount);
        _animator.SetFloat("forward", move.z * forwardModifier);
      } else {
        _animator.SetFloat("horizontal", turnAmount, 0.1f, Time.deltaTime);
        _animator.SetFloat("forward", move.z * forwardModifier, 0.1f, Time.deltaTime);
      }
    }

    void ApplyAdditionalRotation(float fwd, float turnAmount) {
      var turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, fwd);
      transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
    }

    void OnAnimatorMove() {
      if (Time.deltaTime > 0 && useRootMotion) {
        var positionDelta = _animator.deltaPosition;
        positionDelta.y = 0;
        transform.position += positionDelta;
      }

      foreach (var plugin in _plugins) {
        transform.position += plugin.GetExtraMovement(Time.deltaTime);
      }
    }
  }
}