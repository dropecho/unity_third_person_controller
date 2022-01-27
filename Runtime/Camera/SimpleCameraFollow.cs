using UnityEngine;

namespace Dropecho {
  public class SimpleCameraFollow : MonoBehaviour {
    [field: SerializeField] public GameObject follow { get; set; }
    [field: SerializeField] public Vector3 offset { get; set; }
    [field: SerializeField] public bool followRotation { get; set; }

    [field: SerializeField, Range(0, 1)]
    public float positionDamping { get; set; } = 1;
    [field: SerializeField, Range(0, 1)]
    public float lookAtDamping { get; set; } = 0;

    // void FixedUpdate() => Follow();
    void LateUpdate() => Follow();

    void Follow() {
      var delta = Time.deltaTime;

      var toTarget = (follow.transform.position - transform.position).normalized;
      transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(toTarget, Vector3.up), delta / lookAtDamping);

      var newPos = follow.transform.position + (followRotation ? follow.transform.TransformVector(offset) : offset);
      transform.position = Vector3.Lerp(transform.position, newPos, delta / positionDamping);
    }
  }
}