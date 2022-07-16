using UnityEngine;
using UnityEngine.AI;

namespace Dropecho {
  [RequireComponent(typeof(NavMeshAgent))]
  public class NavMeshInputHandler : MonoBehaviour, IInputSource {
    NavMeshAgent _agent;

    void Awake() {
      _agent = GetComponent<NavMeshAgent>();
      _agent.updateRotation = false;
      _agent.updatePosition = false;
    }

    void LateUpdate() => _agent.nextPosition = transform.position;

    // divide to get vel as percentage of max speed, so normalized like player input would be.
    public Vector2 GetInput() => 
      Vector2.ClampMagnitude(new Vector2(_agent.velocity.x, _agent.velocity.z) / _agent.speed, 1);
  }
}