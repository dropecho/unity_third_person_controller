using UnityEngine;
using UnityEngine.Events;

namespace Dropecho {
  public class FallingMovementPlugin : MonoBehaviour, ICharacterMotorPlugin {
    [Tooltip("Distance to detect ground from bottom of character.")]
    [SerializeField] float _groundCheckDistance = 0.2f;
    [Tooltip("Layers to detect as ground.")]
    [SerializeField] LayerMask _groundLayers = 1;
    [Tooltip("Layers to detect as ground.")]
    [SerializeField] float _gravity = -9.5f;

    [Tooltip("Minimum distance before triggering on land event.")]
    [SerializeField] float _minFallHeight = 0.5f;

    [SerializeField] UnityEvent OnFallStart;
    [SerializeField] UnityEvent<float> OnLand;

    float _startHeight;
    float _gravityVel;
    bool _isFalling = false;

    CharacterController _controller;
    void OnEnable() => _controller = GetComponent<CharacterController>();

    public Vector3 GetExtraMovement(float delta) {
      var startedFalling = !isGrounded && !_isFalling;
      var landed = isGrounded && _isFalling;

      if (startedFalling) {
        _startHeight = transform.position.y;
        _isFalling = true;
        OnFallStart?.Invoke();
      }
      if (landed) {
        _gravityVel = 0;
        _isFalling = false;
        var distance = _startHeight - transform.position.y;
        if (distance > _minFallHeight) {
          OnLand?.Invoke(distance);
        }
      }
      if (_isFalling) {
        _gravityVel += _gravity * delta;
        return new Vector3(0, _gravityVel * delta, 0);
      }
      return Vector3.zero;
    }

    bool isGrounded => _controller != null
      ? _controller.isGrounded
      : Physics.CheckSphere(checkOrigin, _groundCheckDistance, _groundLayers, QueryTriggerInteraction.Ignore);

    private Vector3 checkOrigin => transform.position + transform.up * _groundCheckDistance / 2;

    void OnDrawGizmosSelected() {
      Gizmos.color = isGrounded ? Color.green : Color.red;
      Gizmos.DrawWireSphere(transform.position + transform.up * _groundCheckDistance / 2, _groundCheckDistance);
    }
  }
}