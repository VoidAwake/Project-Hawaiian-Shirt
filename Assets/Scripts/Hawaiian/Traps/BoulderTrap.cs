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
    private Animator animator;
    [SerializeField] private float timer;
    [SerializeField] private float cooldown;
    public float boulderSpeed;
    private Vector3 start;
    private Vector3 end;

    // Eddie was here
    BoulderAnimator boulderAnimator;
    bool isRolling = false;
    float speedPercent;
    float timeToMaxSpeed = 1;
    float timeToBeginRolling = 2.35f; // Magic magic numbers

    public override void TriggerTrap()
    {
        base.TriggerTrap();
        if (!triggered)
        {
            //Debug.Log("This lil kiddie named '" + this.gameObject.name + "' has been triggered :0");
            triggered = true;
            isRolling = false;
            speedPercent = 0;

            boulder =  Instantiate(boulderPrefab ,this.transform.position,quaternion.identity);
            animator = boulder.GetComponentInChildren<Animator>();
            boulderAnimator = boulder.GetComponent<BoulderAnimator>();
            AssignTargetFromNode();
            //pro.GetComponent<Projectile>().Initialise(GetDir());

            Invoke("BeginRolling", timeToBeginRolling);
        }
        //anim.ResetTrigger("trigger");
    }

    protected override void Update()
    {
        if (triggered && isRolling)
        {
            if (timer < cooldown)
            {
                if (speedPercent < 1.0f)
                {
                    speedPercent += Time.deltaTime / timeToMaxSpeed;
                    if (speedPercent > 1.0f)
                        speedPercent = 1.0f;
                }

                timer += Time.deltaTime * speedPercent;
                Vector3 prev = boulder.transform.position;
                boulder.transform.position = Vector3.Lerp(start, end, timer / cooldown);
                if (animator)
                {
                    bool movingHorizontally = Mathf.Abs(prev.x - boulder.transform.position.x) > Mathf.Abs(prev.y - boulder.transform.position.y); // Is change in x greater than change in y?
                    float moveSpeed = (prev - boulder.transform.position).magnitude / Time.deltaTime * 0.5f; // Rate of movement = change in position irrespective of deltaTime
                    bool moveSpeedIsNegative = (movingHorizontally && prev.x > boulder.transform.position.x || !movingHorizontally && prev.y > boulder.transform.position.y);
                    animator.SetBool("NegativeSpeed", moveSpeedIsNegative);
                    animator.SetBool("Horizontal", movingHorizontally);
                    animator.speed = moveSpeed;
                }
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
            //Destroy(boulder.gameObject);
            boulderAnimator.PlayParticlesExit();
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

    void BeginRolling()
    {
        animator.SetTrigger("Roll");
        isRolling = true;
        boulderAnimator.BeginRolling();
    }
}
