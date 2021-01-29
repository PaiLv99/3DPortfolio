using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseBTNode : BTNode
{
    private Transform player;
    private NavMeshAgent agent;
    private EnemyAI ai;

    public ChaseBTNode(Transform player, NavMeshAgent agent, EnemyAI ai)
    {
        this.player = player;
        this.agent = agent;
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        float distance = Vector3.Distance(player.position, agent.transform.position);

        if (distance > 0.2f )
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
            ai.Chase(true);
            return NodeState.Running;
        }
        else
        {
            ai.Chase(false);
            agent.isStopped = true;
            return NodeState.Success;
        }
    }
}
