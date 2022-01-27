using UnityEngine;

namespace Dropecho {
  public class SimpleMotor : MonoBehaviour, ICharacterMotor {
    [HelpBox(" This motor moves the object in world space based on input.\n It will rotate the object to face the desired movement direction.")]
    [Tooltip("The speed at which the object moves forward.")]
    public float moveSpeed = 5f;
    [Tooltip("The speed at which the object turns.")]
    public float turnSpeed = 360;

    public void Move(Vector2 input, float forwardModifier = 1) {
      if (input == Vector2.zero) return;

      var move = transform.InverseTransformDirection(new Vector3(input.x, 0, input.y));
      var turnAmount = Mathf.Clamp(Mathf.Atan2(move.x, move.z), -1, 1);
      var fwd = move.z * forwardModifier;

      transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
      transform.position += transform.forward * fwd * moveSpeed * Time.deltaTime;
    }
  }
}