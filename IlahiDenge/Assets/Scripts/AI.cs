using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour {

    public enum AIState {
        Idle, Carrying, Working, Walking, Building
    }


    
    public AIState currentState = AIState.Idle;

    protected NavMeshAgent agent;

    void Awake() {
        agent = GetComponent<NavMeshAgent>();
    }
}
