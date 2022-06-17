using UnityEngine;

public class NPCCollectState : NPCState
{
    NPC npc;
    GameObject point;

    bool pointCollected = false;
    bool pointTakenToBase = false;

    float collectSpeed = 2f;

    public NPCState UpdateState(NPC npc)
    {
        this.npc = npc;
        this.point = npc.currentTarget;

        if (HaveToCollect())
        {
            if (!pointCollected)
            {
                CollectPoint();
            }

            else if (pointCollected && !pointTakenToBase)
            {
                TakePointBackToBase();
            }
            return npc.collectState;
        }

        else if (HaveToWander())
        {
            pointCollected = false;
            pointTakenToBase = false;
            return npc.wanderState;
        }

        else return null;
    }

    public bool HaveToWander()
    {
        return pointCollected && pointTakenToBase;
    }
    public bool HaveToCollect()
    {
        return !pointCollected || !pointTakenToBase;
    }

    // in no case the npc attacks while carrying a point
    public bool HaveToAttack()
    {
        throw new System.NotImplementedException();
    }

    private void CollectPoint()
    {
        // if point is collectable (in range) then collect
        if (npc.ReachedDestination(point.transform.position, npc.CAN_COLLECT_POINT_RANGE) &&
            point.activeSelf)
        {
            point.SetActive(false);
            pointCollected = true;
        }

        // if point is not in range, move to the point
        else
        {
            npc.MoveToDestiation(point.transform.position, collectSpeed);
        }

    }

    private void TakePointBackToBase()
    {
        // if npc in base, drop the point and continue to wander
        if (npc.ReachedDestination(npc.BASE.position, npc.REACHED_BASE_RANGE))
        {
            pointTakenToBase = true;
        }

        // if haven't reached to base yet, continue with collect state to reach it
        else
        {
            npc.MoveToDestiation(npc.BASE.position, collectSpeed);
        }
    }
}