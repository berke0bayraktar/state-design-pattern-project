using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface NPCState
{
    NPCState UpdateState(NPC npc);
    bool HaveToWander();
    bool HaveToCollect();
    bool HaveToAttack();
}
