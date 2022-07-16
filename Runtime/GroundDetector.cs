using UnityEngine;

namespace Dropecho {
  public class GroundDetector : MonoBehaviour {
    [Tooltip("Distance to detect ground from bottom of character.")]
    [SerializeField] float _groundCheckDistance = 0.2f;
    [Tooltip("Layers to detect as ground.")]
    [SerializeField] LayerMask _groundLayers = 1;

    private CharacterState _characterState;
    private CharacterController _controller;
    readonly int _isGroundedHash = CharacterState.NameToHash("isGrounded");

    void OnEnable() {
      _characterState = GetComponent<CharacterState>();
      _controller = GetComponent<CharacterController>();
    }

    void OnValidate() => OnEnable();

    void Update() {
      var isGrounded = _controller != null
          ? _controller.isGrounded
          : Physics.CheckSphere(checkOrigin, _groundCheckDistance, _groundLayers, QueryTriggerInteraction.Ignore);
      _characterState.SetValue(_isGroundedHash, isGrounded);
    }

    bool isGrounded => _characterState.GetValue(_isGroundedHash);
    Vector3 checkOrigin => transform.position + transform.up * _groundCheckDistance / 2;

    void OnDrawGizmosSelected() {
      Gizmos.color = isGrounded ? Color.green : Color.red;
      if (_controller != null) {
        Gizmos.DrawWireCube(transform.position, new Vector3(_controller.radius * 2, 0, _controller.radius * 2));
      } else {
        Gizmos.DrawWireSphere(transform.position + transform.up * _groundCheckDistance / 2, _groundCheckDistance);
      }
    }
  }
}
