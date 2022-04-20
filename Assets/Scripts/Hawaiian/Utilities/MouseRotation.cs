using Hawaiian.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hawaiian.Utilities
{
    public class MouseRotation : MonoBehaviour
    {

        //TODO: Do better
        [SerializeField] private PlayerInput input;

        
        private PlayerAction action;
        private Vector2 rotation;

        private void Awake()
        {
            action = new PlayerAction();

            action.Player.Rotate.performed += ctx => rotation = ctx.ReadValue<Vector2>();
            action.Player.Rotate.canceled += ctx => rotation = Vector2.zero;

        }

        private void OnEnable()
        {
            action.Enable();
        }

        private void OnDisable()
        {
            
            action.Disable();
        }

        // Start is called before the first frame update
  
 

        // Update is called once per frame
        void Update()
        {
            
            // var mouse = Input.mousePosition;
            // var screenPoint = Camera.main.WorldToScreenPoint(this.transform.position);
            // mouse.x -= screenPoint.x;
            // mouse.y -= screenPoint.y;
            // var angle = Mathf.Atan2(mouse.y, mouse.x) * Mathf.Rad2Deg;

            if (input.devices[0].description.deviceClass != "Keyboard")
            {
                Debug.Log(rotation);
               
                    var joystickAngle = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
                    this.transform.rotation = Quaternion.Euler(new Vector3(0,0,joystickAngle));

                
                    //transform.Rotate(new Vector3(0,0,1) * rotation.x*.2f);
                return; 
            }
            

            var mouse = UnityEngine.Input.mousePosition;
            var screenPoint = Camera.main.WorldToScreenPoint(this.transform.position);
            mouse.x -= screenPoint.x;
            mouse.y -= screenPoint.y;
            var angle = Mathf.Atan2(mouse.y, mouse.x) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.Euler(new Vector3(0,0,angle));
            //if (angle > 90 && angle < 190 || angle < -90 && angle > -190) {this.transform.localScale = new Vector3(1,-1,1);} else {this.transform.localScale = new Vector3(1,1,1);}
            
        }
    }
}
