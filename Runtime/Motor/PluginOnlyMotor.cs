using UnityEngine;

namespace Dropecho {
  public class PluginOnlyMotor : MonoBehaviour {
    ICharacterMotorPlugin[] _plugins;
    CharacterController _controller;

    void OnEnable() {
      _plugins = GetComponentsInChildren<ICharacterMotorPlugin>();
      _controller = GetComponent<CharacterController>();
    }

    public void Update() {
      var translation = Vector3.zero;
      foreach (var plugin in _plugins) {
        plugin.GetMovement(Time.deltaTime, out Vector3 t, out Quaternion q);
        translation += t;
        transform.rotation *= q;
      }

      if (_controller != null) {
        _controller?.Move(translation);
      } else {
        transform.position += translation;
      }
    }
  }
}