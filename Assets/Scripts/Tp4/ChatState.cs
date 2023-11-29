using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChatState : IState
{
    private NavMeshAgent navMeshAgent;
    private float conversationTime = 3f;
    private float timer = 0f;

    public ChatState(NavMeshAgent navMeshAgent, float conversationTime, float timer)
    {
        this.navMeshAgent = navMeshAgent;
        this.conversationTime = conversationTime;
        this.timer = 0;
    }

    public void OnEnter(StateController controller)
    {
        navMeshAgent.speed = 0f;
    }

    public void UpdateState(StateController controller)
    {
        timer += Time.deltaTime;
        if (timer >= conversationTime)
        {
            controller.ChangeState(controller.patrolState);
        }
    }

    public void OnHurt(StateController controller)
    {
        // Pas d'action spécifique lorsqu'on est blessé en mode Chat car pas possible
    }

    public void OnExit(StateController controller)
    {
        navMeshAgent.speed = 3.5f;
    }
}
