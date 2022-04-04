using UnityEngine;

namespace Dropecho {
  public class SimpleMotor : MonoBehaviour, ICharacterMotor {
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

    public void Move(Vector2 input, float forwardModifier = 1) {
      if (input == Vector2.zero) return;

      transform.position += new Vector3(input.x, 0, input.y) * (moveSpeed * forwardModifier * Time.deltaTime);
      transform.rotation = Rotation(new Vector3(input.x, 0, input.y));
    }

    void OnEnable() => _plugins = GetComponentsInChildren<ICharacterMotorPlugin>();

    void LateUpdate() {
      foreach (var plugin in _plugins) {
        transform.position += plugin.GetExtraMovement(Time.deltaTime);
      }
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