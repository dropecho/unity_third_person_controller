using UnityEngine;
using UnityEngine.Events;

namespace Dropecho {
  public class StickyPlugin : MonoBehaviour, ICharacterMotorPlugin {
    [Tooltip("Distance to detect ground from bottom of character.")]
    [SerializeField] float _groundCheckDistance = 0.2f;
    [SerializeField] float _stickyForce = 5;
    [Tooltip("Layers to detect as ground.")]
    [SerializeField] LayerMask _stickyLayers = 1;

    public void GetMovement(float delta, out Vector3 translation, out Quaternion rotation) {
      translation = GetTranslation(delta);
      rotation = Quaternion.identity;
    }

    public Vector3 GetTranslation(float delta) {
      if (stickyHit(out RaycastHit hit)) {
        return new Vector3(0, -hit.distance * _stickyForce * Time.fixedDeltaTime, 0);
      }
      return Vector3.zero;
    }

    private bool stickyHit(out RaycastHit hit) {
      return Physics.Raycast(checkOrigin, Vector3.down, out hit, _groundCheckDistance, _stickyLayers, QueryTriggerInteraction.Ignore);
    }

    private Vector3 checkOrigin => transform.position + transform.up * _groundCheckDistance / 2;

    void OnDrawGizmosSelected() {
      Gizmos.color = stickyHit(out RaycastHit hit) ? Color.green : Color.red;
      Gizmos.DrawLine(checkOrigin, checkOrigin + (Vector3.down * _groundCheckDistance));
      // Gizmos.DrawWireSphere(transform.position + transform.up * _groundCheckDistance / 2, _groundCheckDistance);
    }
  }
}