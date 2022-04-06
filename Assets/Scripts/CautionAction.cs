using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Caution")]
public class CautionAction : Action
{
    
    // Start is called before the first frame update
    public override void Act(EnemyBSM bsm)
    {
        Caution(bsm);
    }

    private void Caution(EnemyBSM bsm)
    {
        
    }
}
