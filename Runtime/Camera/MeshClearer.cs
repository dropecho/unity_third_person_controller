using UnityEngine;

public class MeshClearer : MonoBehaviour {
  private RaycastHit[] hits = new RaycastHit[32];

  [field: SerializeField] public LayerMask layersToClear { get; private set; }
  [field: SerializeField] public float radius { get; private set; }
  [field: SerializeField] public GameObject lookAt { get; private set; }
  [field: SerializeField] public Camera camera { get; private set; }

  void Update() {
    var toTarget = (lookAt.transform.position - camera.transform.position);
    var dist = toTarget.magnitude - radius - 1;
    var dir = (toTarget).normalized;

    foreach (var hit in hits) {
      hit.collider?.gameObject?.GetComponent<MeshFader>()?.FadeIn();
    }

    hits = Physics.SphereCastAll(camera.transform.position, radius, dir, dist, layersToClear);
    foreach (var hit in hits) {
      hit.collider?.gameObject?.GetComponent<MeshFader>()?.FadeOut();
    }
  }
}