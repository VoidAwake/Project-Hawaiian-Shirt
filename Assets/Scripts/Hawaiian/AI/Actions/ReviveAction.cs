using UnityEngine;

namespace Hawaiian.AI.Actions
{
    [CreateAssetMenu(menuName = "PluggableAI/Actions/Revive")]
    public class ReviveAction : Action
    {
        public override void Act(EnemyFSM fsm)
        {
            Revive(fsm);
        }

        private void Revive(EnemyFSM fsm)
        {
        
        }
    }
}
