using System.Linq;
using UnityEngine;

namespace Dropecho {
  public class SmoothMotor : MonoBehaviour, ICharacterMotor {
    [HelpBox(
@"This motor moves the object in world space based on input.
It smooths the movement based on acceleration, and so has some interia.
It will rotate the object to face the desired movement direction."
    )]
    [SerializeField, Tooltip("The speed at which the object moves forward.")]
    float _maxSpeed = 5f;
    [SerializeField, Tooltip("The speed at which the object turns.")]
    float _turnSpeed = 360;
    [SerializeField, Range(0f, 100f)]
    float _maxAcceleration = 10f;

    Camera _camera;
    ICharacterMotorPlugin[] _plugins;

    void OnEnable() {
      _camera = Camera.main;
      _plugins = GetComponentsInChildren<ICharacterMotorPlugin>();
    }

    Vector3 velocity;

    public void Move(Vector2 input, float forwardModifier = 1) {
      input = Vector2.ClampMagnitude(input, 1);
      var move = Vector3.ProjectOnPlane(GetCameraRelativeInput(input), Vector3.up);
      var desiredVelocity = move * _maxSpeed;

      var maxSpeedChange = _maxAcceleration * Time.deltaTime;
      velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
      velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);
      transform.position += velocity * Time.deltaTime;

      if (input != Vector2.zero) {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(velocity, Vector3.up), Time.deltaTime * _turnSpeed);
      }
    }

    // void FixedUpdate() {
    //   foreach (var plugin in _plugins) {
    //     // _body.position += plugin.GetExtraMovement(Time.deltaTime);
    //   }
    // }

    Vector3 GetCameraRelativeInput(Vector2 input) {
      var camFwd = Vector3.ProjectOnPlane(_camera.transform.forward, Vector3.up).normalized;
      var worldToCamera = Quaternion.FromToRotation(Vector3.forward, camFwd);
      return worldToCamera * new Vector3(input.x, 0, input.y);
    }
  }
}