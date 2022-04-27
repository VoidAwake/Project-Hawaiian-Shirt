using UnityEngine;
using Hawaiian.Utilities;

namespace Hawaiian.Unit
{
    public class UnitPhysics : MonoBehaviour
    {
        [SerializeField] protected Vector2 gravity = new Vector2(0.0f, -9.8f);
        [SerializeField] protected float minimumDistance = 0.001f;
        [SerializeField] protected float collisionBuffer = 0.01f;
        [SerializeField] protected bool isBoundByCamera = false;
        [SerializeField] protected Collider2D physicsCollider;
        [SerializeField] protected ScriptableFloat gameTimeScale;

        protected Vector2 velocity;
        protected bool isGrounded;
        protected new Rigidbody2D rigidbody;
        protected ContactFilter2D contactFilter;
        protected RaycastHit2D[] hitBuffer = new RaycastHit2D[10]; // Size is arbitrary, but must be larger than however many objects can be collided with in a single fixed update

        // Start is called before the first frame update
        protected virtual void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            contactFilter.useTriggers = false;
            contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
            contactFilter.useLayerMask = true;
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            SetTargetVelocity();
        }

        protected virtual void FixedUpdate()
        {
            // Set and reset working variables
            velocity += gravity * Time.deltaTime * gameTimeScale.Value;
            isGrounded = false;
            Vector2 deltaPosition = velocity * Time.deltaTime * gameTimeScale.Value;

            // Make movement
            if (deltaPosition != null)
            {
                // Horizontal
                Move(Vector2.right * deltaPosition.x, false, true);
                // Vertical
                Move(Vector2.up * deltaPosition.y, true, false);
            }
        }

        protected virtual void Move(Vector2 move, bool isVertical, bool isHorizontal)
        {
            float distance = move.magnitude;

            //if (distance > minimumDistance)
            //{
            int count = physicsCollider.Cast(move, contactFilter, hitBuffer, distance + collisionBuffer);

            for (int i = 0; i < count; i++)
            {
                // Clamp momentum based on normal of face you have collided with
                if (isVertical)
                {
                    if (hitBuffer[i].normal.y > 0.9f) HitWall(0, hitBuffer[i]);
                    else if (hitBuffer[i].normal.y < -0.9f) HitWall(1, hitBuffer[i]);
                }
                else if (isHorizontal)
                {
                    if (hitBuffer[i].normal.x > 0.9f) HitWall(2, hitBuffer[i]);
                    else if (hitBuffer[i].normal.x < -0.9f) HitWall(3, hitBuffer[i]);
                }

                Vector2 normal = hitBuffer[i].normal;

                if (hitBuffer[i].collider.gameObject.tag == "Untagged" || hitBuffer[i].collider.gameObject.tag == gameObject.tag || ((isBoundByCamera && hitBuffer[i].collider.gameObject.tag == "MainCamera")))
                {
                    // Subtract distance you would clip into object from velocity
                    float projection = Vector2.Dot(velocity, normal);
                    if (projection > 0.0f) velocity -= projection * normal;

                    // Use whichever calculated distance is more conservative
                    float modifiedDistance = hitBuffer[i].distance - collisionBuffer;
                    if (distance > modifiedDistance) distance = modifiedDistance;
                }
                //}
            }

            // Apply calculated physics
            rigidbody.position += move.normalized * distance;
        }

        protected virtual void SetTargetVelocity() // This functions exists so that child classes can modify velocity every frame, as necessary
        {

        }

        protected virtual void HitWall(int direction, RaycastHit2D hit) // This functions exists so that additional behaviour can be added by child classes. Directions (of normal) are as follows: 0 up, 1 down, 2 right, 3 left 
        {
            // Cull momentum (and ground, if neccesary) based on normal
            if (hit.collider.gameObject.tag == "Untagged" || (isBoundByCamera && hit.collider.gameObject.tag == "MainCamera"))
            {
                switch (direction)
                {
                    case 0:
                        isGrounded = true;
                        velocity = new Vector2(velocity.x, Mathf.Max(0.0f, velocity.y));
                        break;
                    case 1:
                        velocity = new Vector2(velocity.x, Mathf.Min(0.0f, velocity.y));
                        break;
                    case 2:
                        velocity = new Vector2(Mathf.Max(0.0f, velocity.x), velocity.y);
                        break;
                    case 3:
                        velocity = new Vector2(Mathf.Min(0.0f, velocity.x), velocity.y);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
