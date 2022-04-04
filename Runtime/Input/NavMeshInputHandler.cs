using UnityEngine;
using UnityEngine.AI;

namespace Dropecho {
  [RequireComponent(typeof(ICharacterMotor))]
  [RequireComponent(typeof(NavMeshAgent))]
  public class NavMeshInputHandler : MonoBehaviour {
    ICharacterMotor _motor;
    NavMeshAgent _agent;

    void Awake() {
      _motor = GetComponent<ICharacterMotor>();
      _agent = GetComponent<NavMeshAgent>();

      _agent.updateRotation = false;
      _agent.updatePosition = false;
    }

    void Update() {
      _motor.Move(Vector2.ClampMagnitude(new Vector2(_agent.velocity.x, _agent.velocity.z), 1), 1);
      _agent.nextPosition = transform.position;
    }
  }
}