using UnityEngine;

namespace Dropecho {
  public class RootMotionMotor : MonoBehaviour {
    public bool useRootMotion = true;
    public float moveSpeed = 5;
    public float stationaryTurnSpeed = 180;
    public float movingTurnSpeed = 360;

    Animator _animator;
    ICharacterMotorPlugin[] _plugins;
    CharacterController _controller;
    IInputSource _input;

    void OnEnable() {
      _animator = GetComponentInChildren<Animator>();
      _input = GetComponentInChildren<IInputSource>();
      _controller = GetComponent<CharacterController>();
      _plugins = GetComponentsInChildren<ICharacterMotorPlugin>();
    }

    void Update() {
      var input = _input.GetInput();
      var move = transform.InverseTransformDirection(new Vector3(input.x, 0, input.y));
      var forward = move.z;
      var horizontal = Mathf.Clamp(Mathf.Atan2(move.x, move.z), -1, 1);

      ApplyAdditionalRotation(forward, horizontal);

      if (!useRootMotion) {
        SetPosition(moveSpeed * Time.deltaTime * new Vector3(input.x, 0, input.y));
      }

      _animator.SetFloat("horizontal", horizontal, 0.1f, Time.deltaTime);
      _animator.SetFloat("forward", forward, 0.1f, Time.deltaTime);
    }

    void ApplyAdditionalRotation(float fwd, float turnAmount) {
      var turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, fwd);
      transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
    }

    void SetPosition(Vector3 translation) {
      if (_controller == null) {
        transform.position += translation;
      } else {
        _controller.Move(translation);
      }
    }

    void OnAnimatorMove() {
      Vector3 translation = Vector3.zero;
      if (Time.deltaTime > 0 && useRootMotion) {
        var positionDelta = _animator.deltaPosition;
        positionDelta.y = 0;
        translation += positionDelta;
      }

      foreach (var plugin in _plugins) {
        translation += plugin.GetTranslation(Time.deltaTime);
      }

      SetPosition(translation);
    }
  }
}