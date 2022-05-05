using UnityEngine;

namespace Hawaiian.AI
{
    public class UnitEnemy : Unit.Unit
    {
        EnemyFSM enemyFSM;

        public void OnMove(Vector2 yes) { move = yes; }
    }
}