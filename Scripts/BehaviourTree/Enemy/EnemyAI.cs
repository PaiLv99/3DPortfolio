using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private EnemyController controller;
    private NavMeshAgent agent;
    private Transform coverSpot;
    [SerializeField] private Cover[] avaliableCover;
    [SerializeField] private Transform muzzle;

    private Transform player;
    private BTNode rootNode;

    private void Awake()
    {
        controller = GetComponent<EnemyController>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        CreateBehaviourTree();
    }

    private void Update()
    {
        rootNode.Evaluate();
        if (rootNode.NodeState == NodeState.Failure)
        {
            agent.isStopped = true;
            controller.Idle();
        }
    }

    private float shootDelayTime;
    private float shootTime = 1;
    public void Shoot()
    {
        shootDelayTime += Time.deltaTime;
        if (shootDelayTime >= shootTime)
        {

            shootDelayTime = 0;
            controller.Shoot(player, muzzle);
        }
    }

    private float meleeDelayTime;
    private float meleeTime = 1;
    public void MeleeAttack()
    {
        meleeDelayTime += Time.deltaTime;
        {
            if (meleeDelayTime >= meleeTime)
            {
                meleeDelayTime = 0;
                controller.MeleeAttack(player);
            }
        }
    }

    public void Chase(bool state)
    {
        controller.Chase(state);
    }

    public void SetBestCoverSpot(Transform spot)
    {
        coverSpot = spot;
    }

    public Transform GetBestCoverSpot()
    {
        return coverSpot;
    }

    public virtual void CreateBehaviourTree()
    {
        IsCoveredAvalibleBTNode isCoveredAvalibleBTNode = new IsCoveredAvalibleBTNode(avaliableCover, player, this);
        GoToCoverBTNode goToCoverBTNode = new GoToCoverBTNode(agent, this, controller);
        HealthBTNode healthBTNode = new HealthBTNode(controller);
        IsCoveredBTNode isCoveredBTNode = new IsCoveredBTNode(transform, player);
        ChaseBTNode chaseBTNode = new ChaseBTNode(player, agent, this);
        RangeBTNode chaseRange = new RangeBTNode(controller.ChaseRange, transform, player);
        ShootBTNode shootBTNode = new ShootBTNode(this, agent);
        RangeBTNode shootRangeBTNode = new RangeBTNode(controller.ShootRange, transform, player);
        ObstacleCheck obstacleCheck = new ObstacleCheck(transform, player);
        //IsShootAvailable isShootAvailable = new IsShootAvailable(transform, player);
        RangeBTNode meleeRange = new RangeBTNode(controller.MeleeRange, transform, player);
        MeleeBTNode meleeBTNode = new MeleeBTNode(this, agent);

        BTSequence chaseSequence = new BTSequence(new List<BTNode> { chaseRange, chaseBTNode });
        BTSequence shootSequence = new BTSequence(new List<BTNode> { shootRangeBTNode, obstacleCheck, shootBTNode });
        BTSequence meleeSequence = new BTSequence(new List<BTNode> { meleeRange, obstacleCheck, meleeBTNode });

        BTSequence goToCoverSequence = new BTSequence(new List<BTNode> { isCoveredAvalibleBTNode, goToCoverBTNode });
        BTSelector findCoverSelector = new BTSelector(new List<BTNode> { goToCoverSequence, chaseSequence });
        BTSelector tryToFindSelctor = new BTSelector(new List<BTNode> { isCoveredBTNode, findCoverSelector });
        BTSequence coverSequence = new BTSequence(new List<BTNode> { healthBTNode, tryToFindSelctor});

        rootNode = new BTSelector(new List<BTNode> { coverSequence, meleeSequence, shootSequence, chaseSequence });
    }
}
