using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Revive")]
public class ReviveAction : Action
{
    
    // Start is called before the first frame update
    public override void Act(EnemyFSM fsm)
    {
        Revive(fsm);
    }

    private void Revive(EnemyFSM fsm)
    {
        
    }
}
