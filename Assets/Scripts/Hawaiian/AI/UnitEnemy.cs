using UnityEngine;

namespace Hawaiian.AI
{
    public class UnitEnemy : Unit.Unit
    {
        public void OnMove(Vector2 yes) { move = yes; }
    }
}