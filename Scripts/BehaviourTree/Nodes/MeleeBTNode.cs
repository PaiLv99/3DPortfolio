using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeBTNode : BTNode
{
    private EnemyAI ai;
    private NavMeshAgent agent;

    public MeleeBTNode(EnemyAI ai, NavMeshAgent agent)
    {
        this.ai = ai;
        this.agent = agent;
    }

    public override NodeState Evaluate()
    {
        agent.isStopped = true;
        ai.MeleeAttack();
        return NodeState.Success;
    }
}
