using UnityEngine;

namespace Dropecho {
  public class SimpleMotor : MonoBehaviour {
    [field: HelpBox(
@"This motor moves the object in world space based on input.
It will (optionally) rotate the object to face the desired movement direction."
    )]

    [field: Tooltip("The speed at which the object moves."), SerializeField]
    public float moveSpeed { get; set; } = 5f;
    [field: Tooltip("The speed at which the object rotates."), SerializeField]
    private float turnSpeed { get; set; } = 180f;
    [field: Tooltip("Rotate the object to face movement."), SerializeField]
    public bool rotateToFaceMovement { get; set; } = false;

    ICharacterMotorPlugin[] _plugins;
    IInputSource _input;
    
    void OnEnable() {
      _plugins = GetComponentsInChildren<ICharacterMotorPlugin>();
      _input = GetComponentInChildren<IInputSource>();
    }

    void Update() {
      var input = _input.GetInput();

      var translation = new Vector3(input.x, 0, input.y) * (moveSpeed * Time.deltaTime);
      foreach (var plugin in _plugins) {
        translation += plugin.GetTranslation(Time.deltaTime);
      }
      transform.position += translation;

      transform.rotation = Rotation(new Vector3(input.x, 0, input.y));
    }

    private Quaternion Rotation(Vector3 input) {
      if (rotateToFaceMovement && input.magnitude > 0) {
        var lookRotation = Quaternion.LookRotation(input, transform.up);
        return Quaternion.RotateTowards(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
      }

      return transform.rotation;
    }
  }
}