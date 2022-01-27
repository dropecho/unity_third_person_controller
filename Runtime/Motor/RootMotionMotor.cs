using UnityEngine;

namespace Dropecho {
  [RequireComponent(typeof(Animator))]
  [RequireComponent(typeof(Rigidbody))]
  public class RootMotionMotor : MonoBehaviour, ICharacterMotor {
    [HelpBox("THIS IS A TEST BANANA TIME\n\nWEE")]
    public float stationaryTurnSpeed = 180;
    public float movingTurnSpeed = 360;

    Animator _animator;
    Rigidbody _rigidbody;
    ICharacterMotorPlugin[] _plugins;

    void Awake() {
      _animator = GetComponent<Animator>();
      _rigidbody = GetComponent<Rigidbody>();
      _plugins = GetComponentsInChildren<ICharacterMotorPlugin>();
      _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }

    public void Move(Vector2 input, float forwardModifier = 1) {
      var move = transform.InverseTransformDirection(new Vector3(input.x, 0, input.y));
      var turnAmount = Mathf.Clamp(Mathf.Atan2(move.x, move.z), -1, 1);

      ApplyExtraTurnRotation(move.z, turnAmount);

      _animator.SetFloat("horizontal", turnAmount, 0.1f, Time.deltaTime);
      _animator.SetFloat("forward", move.z * forwardModifier, 0.1f, Time.deltaTime);
    }

    void FixedUpdate() {
      foreach (var plugin in _plugins) {
        _rigidbody.position += plugin.GetExtraMovement();
      }
    }

    // help the character turn faster (this is in addition to root rotation in the animation)
    void ApplyExtraTurnRotation(float fwd, float turnAmount) {
      var turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, fwd);
      transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
    }

    void OnAnimatorMove() {
      if (Time.deltaTime > 0) {
        var velocity = _animator.deltaPosition / Time.deltaTime;
        velocity.y = _rigidbody.velocity.y;
        _rigidbody.velocity = velocity;
      }
    }
  }
}