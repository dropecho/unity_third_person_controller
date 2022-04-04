using UnityEngine;

namespace Dropecho {
  [RequireComponent(typeof(CharacterController))]
  public class CharacterControllerMotor : MonoBehaviour, ICharacterMotor {
    public float moveSpeed = 4f;
    public float turnSpeed = 200f;

    ICharacterMotorPlugin[] _plugins;
    CharacterController _controller;

    void OnEnable() {
      _plugins = GetComponentsInChildren<ICharacterMotorPlugin>();
      _controller = GetComponent<CharacterController>();
    }

    public void Move(Vector2 input, float forwardModifier = 1) {
      var input3d = Vector3.ClampMagnitude(new Vector3(input.x, 0, input.y), 1);
      transform.rotation = Rotation(input3d);

      var translation = moveSpeed * forwardModifier * Time.deltaTime * input3d;
      foreach (var plugin in _plugins) {
        translation += plugin.GetExtraMovement(Time.deltaTime);
      }
      _controller.Move(translation);
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