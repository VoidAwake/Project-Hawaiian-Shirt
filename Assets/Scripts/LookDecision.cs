using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookDecision : Decision
{
    public override bool Decide(EnemyFSM fsm)
    {
        bool targetVisible = Look(fsm);
        return targetVisible;
    }

    public bool Look(EnemyFSM fsm)
    {
        RaycastHit2D hit;
        if (fsm.fov)
        {
            return true;
        }
        return false;
    }
}
