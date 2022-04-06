using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Chase")]
public class ChaseAction : Action
{
    
    // Start is called before the first frame update
    public override void Act(EnemyBSM bsm)
    {
        Chase(bsm);
    }

    private void Chase(EnemyBSM bsm)
    {
        
    }
}
