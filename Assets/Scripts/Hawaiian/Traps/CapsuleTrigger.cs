using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleTrigger : TrapController
{
    // Start is called before the first frame update
    [SerializeField] private TrapController[] kiddies;
    void Start()
    {
        kiddies = this.GetComponentsInChildren<TrapController>();
        Debug.Log((this.GetComponentsInChildren<TrapController>()));

    }

    // Update is called once per frame
    public override void TriggerTrap()
    {
        foreach(TrapController trap in kiddies){
            if (trap != this)
            {
                trap.TriggerTrap();
            }
        }
    }
}
