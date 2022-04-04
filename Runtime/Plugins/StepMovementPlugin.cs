using UnityEngine;

namespace Dropecho {
  public class StepMovementPlugin : MonoBehaviour, ICharacterMotorPlugin {
    [SerializeField] float _stepHeight = 0.5f;
    [SerializeField] float _stepSmooth = 15;
    [SerializeField] float _stepCheckDistance = 0.6f;

    public Vector3 GetExtraMovement(float delta) {
      float move = 0;
      move = GetStepMovement(0);
      if (move == 0) {
        move = GetStepMovement(-45);
      }

      if (move == 0) {
        move = GetStepMovement(45);
      }

      return new Vector3(0, move, 0);
    }

    float GetStepMovement(float angle) {
      var dir = DirectionFromAngle(angle, transform.rotation.eulerAngles.y);
      var stepLowerRay = new Ray(transform.position + Vector3.up * 0.05f, dir);
      var stepUpperRay = new Ray(transform.position + Vector3.up * _stepHeight, dir);

      if (Physics.Raycast(stepLowerRay, out var lowerHit, _stepCheckDistance)) {
        if (Vector3.Angle(Vector3.up, lowerHit.normal) < 70) {
          return 0;
        }
        if (!Physics.Raycast(stepUpperRay, _stepCheckDistance)) {
          return _stepSmooth * Time.deltaTime;
        }
      }
      return 0;
    }

    public static Vector3 DirectionFromAngle(float angle, float initial = 0) {
      var dirAngle = angle + initial;
      return new Vector3(Mathf.Sin(dirAngle * Mathf.Deg2Rad), 0, Mathf.Cos(dirAngle * Mathf.Deg2Rad));
    }

    void OnDrawGizmosSelected() {
      DrawRay(0);
      DrawRay(-45);
      DrawRay(45);
    }

    void DrawRay(float angle) {
      var yRot = transform.rotation.eulerAngles.y;

      var stepLowerRay = new Ray(transform.position + Vector3.up * 0.05f, DirectionFromAngle(angle, yRot));
      var stepUpperRay = new Ray(transform.position + Vector3.up * _stepHeight, DirectionFromAngle(angle, yRot));

      var dist = _stepCheckDistance;

      Gizmos.color = Physics.Raycast(stepLowerRay, dist) ? Color.red : Color.white;
      Gizmos.DrawLine(stepLowerRay.origin, stepLowerRay.origin + stepLowerRay.direction * dist);
      Gizmos.color = Physics.Raycast(stepUpperRay, dist) ? Color.red : Color.white;
      Gizmos.DrawLine(stepUpperRay.origin, stepUpperRay.origin + stepUpperRay.direction * dist);
      Gizmos.color = Color.white;
    }
  }
}