using UnityEngine;

namespace Hawaiian.AI
{
    public abstract class Action : ScriptableObject
    {
        public abstract void Act(EnemyFSM fsm);
    }
}
