using UnityEngine;
#if ENABLE_INPUT_SYSTEM
  using UnityEngine.InputSystem;
#endif

namespace Dropecho {
  public class PlayerInputHandler : MonoBehaviour {
#if ENABLE_INPUT_SYSTEM
    public InputActionReference movement;
    public InputActionReference sprint;
#else
    public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";
    public string sprintButton = "Fire3";
#endif

    public float stationaryTurnSpeed = 180;
    public float movingTurnSpeed = 360;

    ICharacterMotor _motor;

    float _forwardModifier = 1;
    Vector2 _input = Vector2.zero;

    void Awake() {
      _motor = GetComponent<ICharacterMotor>();
    }

    void OnEnable() {
#if ENABLE_INPUT_SYSTEM
      movement.action.Enable();
      sprint.action.Enable();

      movement.action.performed += (ctx) => _input = ctx.ReadValue<Vector2>();
      movement.action.canceled += (ctx) => _input = Vector2.zero;
      sprint.action.performed += _ => _forwardModifier = 2;
      sprint.action.canceled += _ => _forwardModifier = 1;
#endif
      Cursor.lockState = CursorLockMode.Locked;
    }

    void OnDisable() {
#if ENABLE_INPUT_SYSTEM
      movement.action.Disable();
      sprint.action.Disable();
#endif
    }

    void Update() {
#if !ENABLE_INPUT_SYSTEM
      _input.x = Input.GetAxis(horizontalAxis);
      _input.y = Input.GetAxis(verticalAxis);
      _forwardModifier = Input.GetButton(sprintButton) ? 2f : 1f;
#endif
      _motor.Move(_input, _forwardModifier);
    }
  }
}