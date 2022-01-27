using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class MeshFader : MonoBehaviour {
  [field: SerializeField] public Material fadedMaterial { get; private set; }

  Material _original;

  void Awake() {
    _original = GetComponent<MeshRenderer>().sharedMaterial;
  }

  public void FadeOut() {
    GetComponent<MeshRenderer>().material = fadedMaterial;
  }

  public void FadeIn() {
    GetComponent<MeshRenderer>().material = _original;
  }
}