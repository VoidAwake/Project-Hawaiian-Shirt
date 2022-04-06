using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/Patrol")]
public class PatrolAction : Action
{
    public override void Act(EnemyBSM bsm)
    {
        Patrol(bsm);
    }

    private void Patrol(EnemyBSM bsm)
    {
        bsm.currentDestination = bsm.waypointList[bsm.nextWaypoint].transform.position;
        if (Vector2.Distance(bsm.transform.position, bsm.currentDestination) < 1.0f)
        {
            bsm.nextWaypoint = (bsm.nextWaypoint + 1) % bsm.waypointList.Count;
            bsm.currentDestination = bsm.waypointList[bsm.nextWaypoint].transform.position;
        }
        bsm.OnMove(bsm.currentDestination);
    }

   
}
