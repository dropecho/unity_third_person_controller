using UnityEngine;

namespace Dropecho {
  public class SimpleWorldMotor : MonoBehaviour, ICharacterMotor {
    [Tooltip("The speed at which the object moves.")]
    public float moveSpeed = 5f;

    public void Move(Vector2 input, float forwardModifier = 1) {
      if (input == Vector2.zero) return;

      var move = transform.InverseTransformDirection(new Vector3(input.x, 0, input.y));
      move *= forwardModifier;

      transform.position += move * moveSpeed * Time.deltaTime;
    }
  }
}