using UnityEngine;
using UnityEngine.Events;

namespace Dropecho {
  public class FallingMovementPlugin : MonoBehaviour, ICharacterMotorPlugin {
    [Tooltip("The maximum speed to fall at.")]
    [SerializeField] float _maxFallSpeed = 20;
    [Tooltip("Gravity to apply to character.")]
    [SerializeField] float _gravity = -9.5f;
    [Tooltip("Minimum distance before triggering on land distance event.")]
    [SerializeField] float _minFallHeight = 0.5f;

    [SerializeField] UnityEvent OnFallStart;
    [SerializeField] UnityEvent OnLand;
    [SerializeField] UnityEvent<float> OnLandDistance;

    CharacterState _characterState;
    CharacterController _controller;

    readonly int _isGroundedHash = CharacterState.NameToHash("isGrounded");
    bool isGrounded => _characterState.GetValue(_isGroundedHash);

    float _startHeight;
    float _gravityVel;
    bool _isFalling = false;

    void OnEnable() {
      _characterState = GetComponent<CharacterState>();
      _controller = GetComponent<CharacterController>();
    }

    public void GetMovement(float delta, out Vector3 translation, out Quaternion rotation) {
      translation = GetTranslation(delta);
      rotation = Quaternion.identity;
    }

    public Vector3 GetTranslation(float delta) {
      TriggerEvents();

      if (_isFalling) {
        _gravityVel = Mathf.MoveTowards(_gravityVel, -_maxFallSpeed, -_gravity * delta);
      } else {
        _gravityVel = _gravity;
      }
      return new Vector3(0, _gravityVel * delta, 0);
    }

    private void TriggerEvents() {
      var startedFalling = !isGrounded && !_isFalling;
      var landed = isGrounded && _isFalling;

      if (startedFalling) {
        _startHeight = transform.position.y;
        _isFalling = true;
        _gravityVel = 0;
        OnFallStart?.Invoke();
      }

      if (_isFalling && transform.position.y > _startHeight) {
        _startHeight = transform.position.y;
      }

      if (landed) {
        _isFalling = false;
        OnLand?.Invoke();

        var distance = _startHeight - transform.position.y;
        if (distance > _minFallHeight) {
          OnLandDistance.Invoke(distance);
        }
      }
    }

  }
}