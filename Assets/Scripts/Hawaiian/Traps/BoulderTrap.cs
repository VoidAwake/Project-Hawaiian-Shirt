using System.Collections;
using System.Collections.Generic;
using Hawaiian.Unit;
using Unity.Mathematics;
using UnityEngine;

public class BoulderTrap : TrapController
{
    // Start is called before the first frame update
    [SerializeField] private GameObject[] nodes;
    private int counter = 0;
    [SerializeField] private GameObject boulderPrefab;
    private GameObject boulder;
    [SerializeField] private float timer;
    [SerializeField] private float cooldown;
    public float boulderSpeed;
    private Vector3 start;
    private Vector3 end;

    public override void TriggerTrap()
    {
        base.TriggerTrap();
        if (!triggered)
        {
            Debug.Log("This lil kiddie named '" + this.gameObject.name + "' has been triggered :0");
            triggered = true;

            boulder =  Instantiate(boulderPrefab ,this.transform.position,quaternion.identity);
            AssignTargetFromNode();
            //pro.GetComponent<Projectile>().Initialise(GetDir());

        }
        //anim.ResetTrigger("trigger");
    }

    protected override void Update()
    {
        if (triggered)
        {
            if (timer < cooldown)
            {
                timer += Time.deltaTime;
                boulder.transform.position = Vector3.Lerp(start, end, timer / cooldown);
            }
            else
            {
                boulder.transform.position = end;
                AssignTargetFromNode();
            }
        }
    }

    private void AssignTargetFromNode()
    {
        if (counter > nodes.Length - 1)
        {
            Destroy(boulder.gameObject);
            triggered = false;
            counter = 0;
        }
        else
        {
            end = nodes[counter].transform.position;
            start = boulder.transform.position;
            timer = 0f;
            cooldown = Vector3.Distance(start, end) / boulderSpeed;
            counter++;
        }
    }

    // Update is called once per frame
}
