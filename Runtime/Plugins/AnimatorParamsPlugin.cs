using UnityEngine;
#if ENABLE_INPUT_SYSTEM
  using UnityEngine.InputSystem;
#endif

namespace Dropecho {
  public class AnimatorParamsPlugin : MonoBehaviour, ICharacterMotorPlugin {
    public float paramDampTime = 0.1f;
    private IInputSource _inputSource;
    private Animator _animator;
    private CharacterState _state;

    void OnEnable() {
      _inputSource = GetComponentInParent<IInputSource>();
      _animator = GetComponentInParent<Animator>();
      _state = GetComponentInParent<CharacterState>();
    }

    public void GetMovement(float delta, out Vector3 translation, out Quaternion rotation) {
      translation = Vector3.zero;
      rotation = Quaternion.identity;

      var input = _inputSource.GetInput();
      var move = transform.InverseTransformDirection(new Vector3(input.x, 0, input.y));
      var turnAmount = Mathf.Clamp(Mathf.Atan2(move.x, move.z), -1, 1);

      _animator.SetFloat("horizontal", turnAmount, paramDampTime, delta);
      _animator.SetFloat("forward", move.z, paramDampTime, delta);

      // _animator.SetBool("jump", _state.GetValue("isJumping"));
      // _animator.SetBool("grounded", _state.GetValue("isGrounded"));
    }

    public Vector3 GetTranslation(float delta) {
      return Vector3.zero;
    }
  }
}