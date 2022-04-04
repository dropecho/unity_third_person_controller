using UnityEngine;

namespace Dropecho {
  public class SimpleFallingPlugin : MonoBehaviour, ICharacterMotorPlugin {
    [Tooltip("Distance to detect ground from bottom of character.")]
    [SerializeField] float _groundCheckDistance = 0.2f;
    [Tooltip("Layers to detect as ground.")]
    [SerializeField] LayerMask _groundLayers = 1;
    [Tooltip("Layers to detect as ground.")]
    [SerializeField] float _gravity = -9.5f;

    float _vel;

    public Vector3 GetExtraMovement(float delta) => isGrounded
      ? new Vector3(0, _vel = 0, 0)
      : new Vector3(0, (_vel += _gravity * delta) * delta, 0);

    bool isGrounded => Physics.CheckSphere(checkOrigin, _groundCheckDistance, _groundLayers, QueryTriggerInteraction.Ignore);
    private Vector3 checkOrigin => transform.position + transform.up * _groundCheckDistance / 2;

    void OnDrawGizmosSelected() {
      Gizmos.color = isGrounded ? Color.green : Color.red;
      Gizmos.DrawWireSphere(transform.position + transform.up * _groundCheckDistance / 2, _groundCheckDistance);
    }
  }
}