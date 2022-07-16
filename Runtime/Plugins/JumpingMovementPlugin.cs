using UnityEngine;
using UnityEngine.Events;

namespace Dropecho {
  public class JumpingMovementPlugin : MonoBehaviour, ICharacterMotorPlugin {
    [Tooltip("The amount of force to apply when jumping.")]
    [SerializeField] float _jumpForce = 10;
    [Tooltip("Time to allow jumps after a fall starts.")]
    [SerializeField] float _coyoteTime = 0.5f;
    [Tooltip("Minimum time grounded before another jump.")]
    [SerializeField] float _minGroundedTime = 0.1f;

    [SerializeField] UnityEvent OnJump;

    readonly int _isGroundedHash = CharacterState.NameToHash("isGrounded");
    readonly int _isJumpingHash = CharacterState.NameToHash("isJumping");
    bool _isJumping {
      get => _characterState.GetValue(_isJumpingHash);
      set => _characterState.SetValue(_isJumpingHash, value);
    }
    float _vel;
    float _currentJumpTime;
    float _timeSinceLastGrounded;
    CharacterController _controller;
    CharacterState _characterState;

    void OnEnable() {
      _controller = GetComponent<CharacterController>();
      _characterState = GetComponent<CharacterState>();
    }

    public void GetMovement(float delta, out Vector3 translation, out Quaternion rotation) {
      translation = GetTranslation(delta);
      rotation = Quaternion.identity;
    }

    public Vector3 GetTranslation(float delta) {
      if (isGrounded) {
        _timeSinceLastGrounded = 0;
        _vel = 0;
        if (_currentJumpTime > _minGroundedTime) {
          _currentJumpTime = 0;
          _isJumping = false;
        }
      } else {
        _timeSinceLastGrounded += delta;
      }

      if (isAllowedToJump && Input.GetButton("Jump")) {
        Debug.Log("wee jump");

        _vel = _jumpForce;
        _isJumping = true;
        OnJump?.Invoke();
      }

      if (_isJumping) {
        _currentJumpTime += delta;
        if (!isGrounded) {
          _vel = Mathf.Lerp(_vel, 0, _currentJumpTime / _jumpForce);
        }
      }

      return new Vector3(0, _vel * delta, 0);
    }

    bool isAllowedToJump => _timeSinceLastGrounded < _coyoteTime && !_isJumping;
    bool isGrounded => _characterState.GetValue(_isGroundedHash);
  }
}