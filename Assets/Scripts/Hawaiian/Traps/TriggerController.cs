using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] TrapController[] kiddies;
    
    private void OnTrigger()
    {
        foreach(TrapController trap in kiddies){
            trap.TriggerTrap();
        }
    }



    void OnTriggerEnter2D(Collider2D col) {
        Debug.Log("Big Daady has been triggered!!!!");
        OnTrigger();
    }
}
