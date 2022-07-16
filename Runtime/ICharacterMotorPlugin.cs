using UnityEngine;

namespace Dropecho {
  public interface ICharacterMotorPlugin {
    Vector3 GetTranslation(float delta);
    void GetMovement(float delta, out Vector3 translation, out Quaternion rotation);
  }
}