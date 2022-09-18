using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : TrapController
{
    // Start is called before the first frame update
    [SerializeField] private PolygonCollider2D spike;
    [SerializeField] private float activeTime;
    private float timer = 0f;
    [SerializeField] private Animator anim;


    protected override void Update()
    {
        if (triggered)
        {
            timer += Time.deltaTime;
            if (timer > activeTime)
            {
                triggered = false;
                spike.enabled = false;
                anim.SetBool("active", false);
                timer = 0f;
            }
        }
    }

    public override void TriggerTrap()
    {
        triggered = true;
        spike.enabled = true;
        anim.SetBool("active", true);

    }

    // Update is called once per frame
    
}
