using UnityEngine;

namespace Hawaiian.AI
{
    [CreateAssetMenu(menuName = "PluggableAI/Actions/Chase")]
    public class ChaseAction : Action
    {
        public override void Act(EnemyFSM fsm)
        {
            Chase(fsm);
        }

        private void Chase(EnemyFSM fsm)
        {
        
        }
    }
}
