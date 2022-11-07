using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hawaiian.Unit
{
    public class BombPhysics : UnitPhysics
    {
        float velocityLifetime = 0.5f;
        float decelerationRate = 9.0f;

        protected override void SetTargetVelocity() // This functions exists so that child classes can modify velocity every frame, as necessary
        {
            print("bomb: " + velocity);

            if (velocityLifetime > 0.0f)
            {
                velocityLifetime -= Time.deltaTime * gameTimeScale.Value;
                return;
            }

            if (velocity.magnitude > 0.0f)
            {
                float newSpeed = (velocity.magnitude - (decelerationRate * Time.deltaTime * gameTimeScale.Value)) / velocity.magnitude;
                if (newSpeed <= 0.0f)
                {
                    print("huh");
                    newSpeed = 0.0f;
                }
                velocity *= newSpeed;
            }
        }

        protected override void HitWall(int direction, RaycastHit2D hit) // This functions exists so that additional behaviour can be added by child classes. Directions (of normal) are as follows: 0 up, 1 down, 2 right, 3 left 
        {
            // Rebound based on normal
            if (hit.collider.gameObject.tag == "Untagged" || (isBoundByCamera && hit.collider.gameObject.tag == "MainCamera"))
            {
                print("bomb 1: " + velocity);

                switch (direction)
                {
                    case 0:
                        velocity = new Vector2(velocity.x, -velocity.y);
                        break;
                    case 1:
                        velocity = new Vector2(velocity.x, -velocity.y);
                        break;
                    case 2:
                        velocity = new Vector2(-velocity.x, velocity.y);
                        break;
                    case 3:
                        velocity = new Vector2(-velocity.x, velocity.y);
                        break;
                    default:
                        break;
                }

                print("bomb 2: " + velocity);
            }
        }
    }
}