using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : TrapController
{
    // Start is called before the first frame update
    [SerializeField] GameObject projectile;

    public override void TriggerTrap()
    {
        base.TriggerTrap();
        Debug.Log("This lil kiddie named: " + this.gameObject.name +" has been triggered :0");
    }
}
