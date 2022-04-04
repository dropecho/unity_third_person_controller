using UnityEngine;

namespace Dropecho {
  public interface ICharacterMotorPlugin {
    Vector3 GetExtraMovement(float delta);
  }
}