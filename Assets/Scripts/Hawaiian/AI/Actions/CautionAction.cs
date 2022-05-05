using UnityEngine;

namespace Hawaiian.AI
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
