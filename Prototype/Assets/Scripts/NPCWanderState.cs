using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWanderState : NPCState
{
    NPC npc;
    Vector2 waypoint;

    bool waypointSet = false;
    const float freedomMin = 2f, freedomMax = 8f;
    const float xMin = -8.4f, xMax = 8.4f;
    const float yMin = -4.5f, yMax = 4.5f;
    const float wanderSpeed = 2f;
    bool reachedDestination = false;

    public NPCState UpdateState(NPC npc)
    {
        this.npc = npc;

        if (HaveToCollect())
        {
            waypointSet = false;
            return npc.collectState;
        }

        else if (HaveToAttack())
        {
            waypointSet = false;
            return npc.attackState;
        }

        else if (HaveToWander())
        {
            Wander();
            return npc.wanderState;
        }
        return null;
    }
    public bool HaveToWander()
    {
        return !waypointSet || !reachedDestination;
    }
    public bool HaveToCollect()
    {
        return npc.Nearby(npc.points, npc.POINT_SEARCH_RANGE, setTarget:true);
    }

    public bool HaveToAttack()
    {
        return npc.Nearby(npc.enemies, npc.ENEMY_SEARCH_RANGE, setTarget:true);
    }

    private void Wander()
    {
        if (!waypointSet)
        {
            SetNewWaypoint();
            waypointSet = true;
        }

        reachedDestination = npc.ReachedDestination(waypoint, npc.WAYPOINT_REACHED_RANGE);

        if (!reachedDestination)
        {
            npc.MoveToDestiation(waypoint, wanderSpeed);
        }
        else
        {
            waypointSet = false;
        }
    }

    private void SetNewWaypoint()
    {
        float xFreedom = UnityEngine.Random.Range(freedomMin, freedomMax);
        float yFreedom = UnityEngine.Random.Range(freedomMin, freedomMax);

        float newX = UnityEngine.Random.Range(npc.transform.position.x - xFreedom,
            npc.transform.position.x + xFreedom);

        float newY = UnityEngine.Random.Range(npc.transform.position.y - yFreedom,
            npc.transform.position.y + yFreedom);

        if (newX > xMax) { newX = xMax; }
        else if (newX < xMin) { newX = xMin; }

        if (newY > yMax) { newY = yMax; }
        else if (newY < yMin) { newY = yMin; }

        waypoint = new Vector2(newX, newY);
    }
}
