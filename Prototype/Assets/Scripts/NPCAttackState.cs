using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class NPCAttackState : NPCState
{
    public NPC npc;
    public GameObject enemy;
    const float enemyChaseSpeed = 2f;
    bool enemyDead = false;
    long attackTimeIntervalMS = 500;
    long timeAtLastAttack = currentTime();

    public NPCState UpdateState(NPC npc)
    {
        this.npc = npc;
        this.enemy = npc.currentTarget;

        if (HaveToAttack())
        {
            Attack();
            return npc.attackState;
        }

        else if (HaveToWander())
        {
            enemyDead = false;
            return npc.wanderState;
        }

        else return null;
    }

    public bool HaveToWander()
    {
        return enemyDead;
    }

    public bool HaveToAttack()
    {
        return !enemyDead;
    }

    // in no case the npc will switch from attack state to collect state
    public bool HaveToCollect()
    {
        // TODO BERKAY
        throw new System.NotImplementedException();
    }

    private void Attack()
    {
        // if enemy in attack range, attack it
        if (npc.ReachedDestination(enemy.transform.position, npc.ATTACK_ENEMY_RANGE) && timePassed())
        {
            enemy.GetComponent<NPC>().health -= 10f;
            timeAtLastAttack = currentTime();
            
            // if enemy dead after new attack
            if (enemy.GetComponent<NPC>().health <= 0)
            {
                enemyDead = true;
                enemy.SetActive(false);
            }
        }

        // if not in range, move to enemy
        else
        {
            npc.MoveToDestiation(enemy.transform.position, enemyChaseSpeed);
        }
    }

    static long currentTime()
    {
        return new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();
    }

    bool timePassed()
    {
        return currentTime() - timeAtLastAttack > attackTimeIntervalMS;
    }
}
