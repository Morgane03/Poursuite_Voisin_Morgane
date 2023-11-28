using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public interface IState
{
    public void OnEnter(StateController controller);
    public void UpdateState(StateController controller);
    public void OnHurt(StateController controller);
    public void OnExit(StateController controller);
}

public class StateController : MonoBehaviour
{
    IState currentState;

    public PursuingState chaseState;
    public PatrolState patrolState;
    public ChatState chatState = new ChatState();

    public Transform[] patrolWaypoints;
    public float pursueDistance = 5f; // Distance à partir de laquelle le garde passe en mode Poursuite
    public GameObject player;


    public void Start()
    {
        patrolState = new PatrolState(patrolWaypoints);
        ChangeState(patrolState);
    }

    void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateState(this);
        }
        if (MathHelper.VectorDistance(transform.position, player.transform.position) < pursueDistance)
        {
            // Passer en mode poursuite
            ChangeState(new PursuingState(GetComponent<NavMeshAgent>(), player.transform));
        }
        if (MathHelper.VectorDistance(transform.position, player.transform.position) <= pursueDistance && currentState != chaseState)
        {
            ChangeState(chatState);
        }
    }

    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }
        currentState = newState;
        currentState.OnEnter(this);
    }
}
