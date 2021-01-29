using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShootBTNode : BTNode
{
    private NavMeshAgent agent;
    private EnemyAI ai;

    public ShootBTNode(EnemyAI ai, NavMeshAgent agent)
    {
        this.ai = ai;
        this.agent = agent;
    }

    public override NodeState Evaluate()
    {
        agent.isStopped = true;
        ai.Shoot();
        return NodeState.Success;
    }
}
