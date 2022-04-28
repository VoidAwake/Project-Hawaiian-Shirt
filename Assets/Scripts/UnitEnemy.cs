using System.Collections;
using System.Collections.Generic;
using Hawaiian.Unit;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hawaiian.Unit
{
    public class UnitEnemy : Unit
    {
        EnemyFSM enemyFSM;

        // Start is called before the first frame update
        public void OnMove(Vector2 yes) { move = yes; }

    }
}