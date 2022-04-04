using UnityEngine;

namespace Dropecho {
  public class JumpingMovementPlugin : MonoBehaviour, ICharacterMotorPlugin {
    [Tooltip("Distance to detect ground from bottom of character.")]
    [SerializeField] float _groundCheckDistance = 0.2f;
    [Tooltip("Layers to detect as ground.")]
    [SerializeField] LayerMask _groundLayers = 1;
    [Tooltip("Layers to detect as ground.")]
    [SerializeField] float _jumpForce = 10;

    bool _isJumping = false;
    float _vel;
    float _timeSinceJumpStarted;

    CharacterController _controller;
    void OnEnable() => _controller = GetComponent<CharacterController>();

    public Vector3 GetExtraMovement(float delta) {
      if (isGrounded) {
        if (_timeSinceJumpStarted > 0.2f) {
          _vel = 0;
          _timeSinceJumpStarted = 0;
          _isJumping = false;
        } else if (Input.GetButton("Jump") && !_isJumping) {
          _vel = _jumpForce;
          _isJumping = true;
        }
      }

      if (_isJumping) {
        _timeSinceJumpStarted += delta;
        _vel = Mathf.Lerp(_vel, 0, _timeSinceJumpStarted / _jumpForce);
      }

      return new Vector3(0, _vel * delta, 0);
    }

    bool isGrounded => _controller != null
      ? _controller.isGrounded
      : Physics.CheckSphere(checkOrigin, _groundCheckDistance, _groundLayers, QueryTriggerInteraction.Ignore);

    Vector3 checkOrigin => transform.position + transform.up * _groundCheckDistance / 2;

    void OnDrawGizmosSelected() {
      Gizmos.color = isGrounded ? Color.green : Color.red;
      Gizmos.DrawWireSphere(transform.position + transform.up * _groundCheckDistance / 2, _groundCheckDistance);
    }
  }
}