using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CascadeTrap : TrapController
{
    [SerializeField] private float intervalTime;
    // Start is called before the first frame update
    [SerializeField] TrapController[] kiddies;
    private float timer = 0.0f;
    public float timeBetweenTrigger;
    public int pos = 0;
    public bool loop;
    
    public override void TriggerTrap()
    {
        triggered = true;
    }

    protected override void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            if (kiddies[pos] != null)
            {
                kiddies[pos].TriggerTrap();
                pos += 1;
            }

            if (pos > kiddies.Length - 1)
            {
                pos = 0;
                if (!loop)
                {
                    triggered = false;
                }
            }
        }
    }
}