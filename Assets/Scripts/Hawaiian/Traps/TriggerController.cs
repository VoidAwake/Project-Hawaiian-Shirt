using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] TrapController[] kiddies;
    public bool test;
    
    private void OnTrigger()
    {
        foreach(TrapController trap in kiddies){
            trap.TriggerTrap();
        }
    }

    void Update()
    {
        if (test)
        {
            test = !test;
            foreach(TrapController trap in kiddies){
                Debug.DrawLine(transform.position,trap.transform.position, Color.red,20f);
                Debug.Log(trap.transform.position);
            }
        }
    }


    void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "Player")
        {
            Debug.Log("Big Daady has been triggered!!!!");
            OnTrigger();
        }
    }
}
