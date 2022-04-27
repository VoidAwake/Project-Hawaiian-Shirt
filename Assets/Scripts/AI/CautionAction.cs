using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Caution")]
public class CautionAction : Action
{
    
    // Start is called before the first frame update
    public override void Act(EnemyFSM fsm)
    {
        Caution(fsm);
    }

    private void Caution(EnemyFSM fsm)
    {
        
    }
}
