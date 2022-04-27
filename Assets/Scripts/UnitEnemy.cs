using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitEnemy : Unit
{
    EnemyFSM enemyFSM;

    // Start is called before the first frame update
    public void OnMove(Vector2 yes) { move = yes; }
    
}
