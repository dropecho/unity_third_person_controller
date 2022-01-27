using UnityEngine;

namespace Dropecho {
  public interface ICharacterMotor {
    void Move(Vector2 input, float forwardModifier = 1);
  }
}