using UnityEngine;

namespace Dropecho {
  [RequireComponent(typeof(CharacterController))]
  public class CharacterControllerMotor : MonoBehaviour {
    public float moveSpeed = 4f;
    public float turnSpeed = 200f;

    ICharacterMotorPlugin[] _plugins;
    CharacterController _controller;
    IInputSource _input;

    void OnEnable() {
      _plugins = GetComponentsInChildren<ICharacterMotorPlugin>();
      _controller = GetComponent<CharacterController>();
      _input = GetComponent<IInputSource>();
    }

    public void Update() {
      var input = _input.GetInput();
      var input3d = Vector3.ClampMagnitude(new Vector3(input.x, 0, input.y), 1);

      var translation = moveSpeed * Time.deltaTime * input3d;
      foreach (var plugin in _plugins) {
        translation += plugin.GetTranslation(Time.deltaTime);
      }
      _controller.Move(translation);
      transform.rotation = Rotation(input3d);
    }

    private Quaternion Rotation(Vector3 input) {
      if (input.magnitude > 0) {
        var lookRotation = Quaternion.LookRotation(input, transform.up);
        return Quaternion.RotateTowards(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
      }
      return transform.rotation;
    }
  }
}