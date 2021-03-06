using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/Patrol")]
public class PatrolAction : Action
{
    public override void Act(EnemyFSM fsm)
    {
        Patrol(fsm);
    }

    private void Patrol(EnemyFSM fsm)
    {
        fsm.currentDestination = fsm.waypointList[fsm.nextWaypoint].transform.position;
        if (Vector2.Distance(fsm.transform.position, fsm.currentDestination) < 1.0f)
        {
            fsm.nextWaypoint = (fsm.nextWaypoint + 1) % fsm.waypointList.Count;
            fsm.currentDestination = fsm.waypointList[fsm.nextWaypoint].transform.position;
        }
        fsm.OnMove(fsm.currentDestination);
    }

   
}
