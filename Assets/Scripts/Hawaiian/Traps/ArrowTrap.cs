using Hawaiian.Inventory.ItemBehaviours;
using UnityEngine;

public class ArrowTrap : TrapController
{
    // Start is called before the first frame update
    [SerializeField] GameObject projectile;
    [SerializeField] private float cooldown;
    [SerializeField] private float timer = 0f;
    [SerializeField] private SpriteRenderer spr;
    private float rotation;

    [SerializeField] private Animator anim;
    [SerializeField] private float projectileDistance;
    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    };

    [SerializeField] private Direction dir;

    public override void TriggerTrap()
    {
        base.TriggerTrap();
        if (!triggered)
        {
            Debug.Log("This lil kiddie named '" + this.gameObject.name + "' has been triggered :0");
            anim.SetTrigger("trigger");
            triggered = true;
            timer = 0;
            Vector3 x = GetDir();
            GameObject pro =  Instantiate(projectile,transform.position + new Vector3(0.1f,0.1f,0.1f),Quaternion.Euler(0f,0f,rotation));
            pro.GetComponent<Projectile>().Initialise(x);

        }
        //anim.ResetTrigger("trigger");
    }

    protected override void Update()
    {
        if (triggered)
        {
            
            spr.material.SetFloat("_time", timer/cooldown - 0.5f);
            timer += Time.deltaTime;
        }

        if (timer > cooldown)
        {
            spr.material.SetFloat("_time", 0.5f);
            triggered = false;
            anim.SetTrigger("trigger");
            timer = 0f;
        }
    }


    private Vector3 GetDir()
    {
        Vector3 x = new Vector3(0f,0f,0f);

        if ( dir == Direction.Up)
        {
            x = new Vector3(0f, 1f, 0f);
            rotation = 0f;
        }
        else if (dir == Direction.Down)
        {
            x = new Vector3(0f, -1f, 0f);
            rotation = 180f;
        }
        else if (dir == Direction.Left)
        {
            x = new Vector3(-1f, 0f, 0f);
            rotation = 90f;
        }
        else if (dir == Direction.Right)
        {
            x = new Vector3(1f, 0f, 0f);
            rotation = 270f;
        }
        //Debug.Log("GET DIR IS" + (transform.position + x * projectileDistance));
        return transform.position + x * projectileDistance;
    }
    
}
