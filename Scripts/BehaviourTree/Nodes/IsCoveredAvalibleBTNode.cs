using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsCoveredAvalibleBTNode : BTNode
{
    private Cover[] covers;
    private Transform player;
    private EnemyAI ai;

    public IsCoveredAvalibleBTNode(Cover[] cover, Transform player, EnemyAI ai)
    {
        this.covers = cover;
        this.player = player;
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        Transform spot = FindBestSpot();
        ai.SetBestCoverSpot(spot);
        return spot != null ? NodeState.Success : NodeState.Failure;
    }

    private Transform FindBestSpot()
    {
        if (ai.GetBestCoverSpot() != null)
        {
            if (Check(ai.GetBestCoverSpot()))
                return ai.GetBestCoverSpot();
        }


        float minAngle = 90;
        Transform best = null;

        for (int i= 0; i < covers.Length; i++)
        {
            Transform bestSpot = FindBestSpotInCover(covers[i], ref minAngle);
            if (bestSpot != null)
                best = bestSpot;
        }

        return best;
    }

    private Transform FindBestSpotInCover(Cover cover, ref float minAngle)
    {
        Transform[] availiableSpot = cover.GetCoverSpot();
        Transform best = null;

        for (int i = 0; i < covers.Length; i++)
        {
            Vector3 dir = player.transform.position - availiableSpot[i].position;

            if (Check(availiableSpot[i]))
            {
                float angle = Vector3.Angle(availiableSpot[i].forward, dir);
                if (angle < minAngle)
                {
                    minAngle = angle;
                    best = availiableSpot[i];
                }
            }
        }
        return best;
    }

    private bool Check(Transform spot)
    {
        RaycastHit hit;
        Vector3 dir = player.transform.position - spot.position;
        if (Physics.Raycast(spot.position, dir, out hit))
        {
            if (hit.transform != player)
                return true;
        }
        return false;
    }
}
