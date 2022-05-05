using UnityEngine;

namespace Hawaiian.AI.Actions
{
    [CreateAssetMenu(menuName = "PluggableAI/Actions/Patrol")]
    public class PatrolAction : Action
    {
        public override void Act(EnemyFSM fsm)
        {
            Patrol(fsm);
        }

        private void Patrol(EnemyFSM fsm)
        {
            if (Vector2.Distance(fsm.transform.position, fsm.CurrentDestination) < 0.5f)
                fsm.ToNextWaypoint();

            fsm.unitEnemy.OnMove(fsm.CurrentDestination - (Vector2) fsm.transform.position);
        }
    }
}
