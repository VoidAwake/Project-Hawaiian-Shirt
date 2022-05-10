using UnityEngine;

namespace Hawaiian.AI.Actions
{
    [CreateAssetMenu(menuName = "PluggableAI/Actions/Caution")]
    public class CautionAction : Action
    {
        public override void Act(EnemyFSM fsm)
        {
            Caution(fsm);
        }

        private void Caution(EnemyFSM fsm)
        {
        
        }
    }
}
