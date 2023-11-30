using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


// explication base avec une interface venu du site https://gamedevbeginner.com/state-machines-in-unity-how-and-when-to-use-them/
public interface IState
{
    public void OnEnter(StateController controller);
    public void UpdateState(StateController controller);
    public void OnHurt(StateController controller);
    public void OnExit(StateController controller);
}

public class StateController : MonoBehaviour
{
    public IState currentState;

    public PursuingState chaseState;
    public PatrolState patrolState;
    public ChatState chatState;
    public AlerteState alerteState;

    public Transform[] patrolWaypoints;
    public float pursueDistance = 5f; // Distance à partir de laquelle le garde passe en mode Poursuite
    public float chatDistance = 3f; // Distance à partir de laquelle le garde passe en mode Chat
    public GameObject player;
    public GameObject guard;


    public void Start()
    {
        patrolState = new PatrolState(patrolWaypoints);
        chaseState = new PursuingState(this.GetComponent<NavMeshAgent>(), player.transform);
        chatState = new ChatState(this.GetComponent<NavMeshAgent>(), 3f, 0f);
        alerteState = new AlerteState(this);
        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.OnNoiseMade += ActivateAlertState;
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
        Debug.Log(currentState);
        currentState.OnEnter(this);
    }

    void ActivateAlertState()
    {
        ChangeState(alerteState);
    }
}
