using UnityEngine;
#if ENABLE_INPUT_SYSTEM
  using UnityEngine.InputSystem;
#endif

namespace Dropecho {
  public enum InputMode {
    world = 1,
    local = 2,
    camera = 3
  }

  public class PlayerInputHandler : MonoBehaviour, IInputSource {
    public InputMode inputMode = InputMode.world;

#if ENABLE_INPUT_SYSTEM
    public InputActionReference movement;
    public InputActionReference sprint;
#else
    public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";
    public string sprintButton = "Fire3";
#endif

    Camera _camera;
    float _forwardModifier = 1;
    Vector2 _input = Vector2.zero;
    Vector2 _processed = Vector2.zero;

    void Awake() => _camera = Camera.main;

    void OnEnable() {
#if ENABLE_INPUT_SYSTEM
      movement.action.Enable();
      sprint.action.Enable();

      movement.action.performed += (ctx) => _input = ctx.ReadValue<Vector2>();
      movement.action.canceled += (ctx) => _input = Vector2.zero;
      sprint.action.performed += _ => _forwardModifier = 2;
      sprint.action.canceled += _ => _forwardModifier = 1;
#endif
      // Cursor.lockState = CursorLockMode.Locked;
    }

    void OnDisable() {
#if ENABLE_INPUT_SYSTEM
      movement.action.Disable();
      sprint.action.Disable();
#endif
    }

    Vector2 ProcessInput(Vector2 _input) {
      return inputMode switch {
        InputMode.world => _input,
        InputMode.local => GetLocalRelativeInput(_input),
        InputMode.camera => GetCameraRelativeInput(_input),
        _ => Vector2.zero
      };
    }

    Vector2 GetLocalRelativeInput(Vector2 input) {
      var localDir = transform.TransformDirection(new Vector3(_input.x, 0, input.y));
      return new Vector2(localDir.x, localDir.z);
    }

    Vector2 GetCameraRelativeInput(Vector2 input) {
      var camFwd = Vector3.ProjectOnPlane(_camera.transform.forward, Vector3.up).normalized;
      var worldToCamera = Quaternion.FromToRotation(Vector3.forward, camFwd);
      var relativeDir = worldToCamera * new Vector3(input.x, 0, input.y);

      return new Vector2(relativeDir.x, relativeDir.z);
    }

    public Vector2 GetInput() {
#if !ENABLE_INPUT_SYSTEM
      _input.x = Input.GetAxis(horizontalAxis);
      _input.y = Input.GetAxis(verticalAxis);
      _forwardModifier = Input.GetButton(sprintButton) ? 1.25f : 1f;
#endif
      return Vector2.ClampMagnitude(ProcessInput(_input), 1) * _forwardModifier;
    }
  }
}