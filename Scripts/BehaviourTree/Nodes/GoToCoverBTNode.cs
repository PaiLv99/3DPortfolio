using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoToCoverBTNode : BTNode
{
    private NavMeshAgent agent;
    private EnemyController controller;
    private EnemyAI ai;

    public GoToCoverBTNode(NavMeshAgent agent, EnemyAI ai, EnemyController controller)
    {
        this.agent = agent;
        this.ai = ai;
        this.controller = controller;
    }

    public override NodeState Evaluate()
    {
        Transform cover = ai.GetBestCoverSpot();

        if (cover == null)
            return NodeState.Failure;

        float distance = Vector3.Distance(cover.transform.position, agent.transform.position);
        if (distance > 0.2f)
        {
            agent.isStopped = false;
            agent.SetDestination(cover.position);
            return NodeState.Running;
        }
        else
        {
            agent.isStopped = true;
            controller.Heal();
            return NodeState.Success;
        }    
    }
}
