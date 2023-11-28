using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

    //public PursuingState chaseState = new PursuingState();
    public PatrolState patrolState;
    //public HurtState hurtState = new HurtState();

    public Transform[] patrolWaypoints;
    public float pursueDistance = 10f; // Distance à partir de laquelle le garde passe en mode Poursuite
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
