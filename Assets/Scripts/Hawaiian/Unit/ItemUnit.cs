using Hawaiian.Unit;
using UnityEngine;

public class ItemUnit : UnitPhysics
{
    // Start is called before the first frame update
    [SerializeField] private bool moving;
    //[SerializeField] private float timer;
    private Vector2 move;
    [SerializeField] private float speed;
    [SerializeField] private float deceleration;    

    protected override void Update()
    {
        base.Update();
        /*if (timer < 0)
        {
            Destroy(this);
        }*/
    }

    public void OnThrow(Vector2 dir)
    {
        move = dir.normalized;
        moving = true;
        velocity = move*speed;
    }
    
    public void OnThrow(Vector2 dir, float speedMultiplier)
    {
        move = dir.normalized;
        moving = true;
        velocity = move* (speed * speedMultiplier);
    }

    protected override void SetTargetVelocity()
    {
        if (moving)
        {
            //Debug.Log(moving);
            //timer -= Time.deltaTime;
            bool epic = velocity.x > 0f;
                velocity -= velocity.normalized * (deceleration * gameTimeScale.Value * Time.deltaTime);
                //velocity *= 0.95f*deceleration;
                // Debug.Log(velocity);
                //im reset to 0
                if (velocity.x > 0f != epic)
                {
                    velocity = Vector2.zero;
                    //  Destroy(this);
                }

                //Vector2 modifiedMove = move.magnitude * 1.2f > 1.0f ? move.normalized : move.magnitude < 0.05f ? Vector2.zero : move * 1.2f;
        //velocity = Vector2.Lerp(velocity, modifiedMove, Mathf.Clamp(Time.deltaTime * gameTimeScale.Value * speed, 0.0f, 1.0f));
        //velocity = Vector2.Lerp(velocity, Vector2.zero, Mathf.Clamp(Time.deltaTime * gameTimeScale.Value * speed, 0.0f, 1.0f));
        }
    }
}
