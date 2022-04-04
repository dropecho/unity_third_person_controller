using UnityEngine;

public class MyTriggeredMover : MonoBehaviour {
  [field: SerializeField] public float moveSpeed { get; set; } = 1f;
  [field: SerializeField] public Vector3 moveTo { get; set; }
  [field: SerializeField] public bool shouldMove { get; set; } = false;
  [field: SerializeField] public float moveDelay { get; set; } = 0f;

  Vector3 _startPos;
  float _moveTime;
  bool _reverse;

  void Awake() => _startPos = transform.position;
  void OnTriggerEnter() => shouldMove = true;
  void OnCollisionEnter() => shouldMove = true;
  void Update() => MoveTo(_reverse ? _startPos : moveTo);

  void MoveTo(Vector3 point) {
    if (shouldMove) {
      _moveTime += Time.deltaTime;
      if (_moveTime > moveDelay) {
        transform.position = Vector3.MoveTowards(transform.position, point, Time.deltaTime * moveSpeed);
        if (Vector3.Distance(transform.position, point) <= 0f) {
          shouldMove = false;
          _reverse = !_reverse;
          _moveTime = 0;
        }
      }
    }
  }
}