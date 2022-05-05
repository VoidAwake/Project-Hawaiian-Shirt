using UnityEngine;

namespace Hawaiian.AI
{
    public abstract class Decision : ScriptableObject {
        public abstract bool Decide(EnemyFSM fsm);
    }
}
