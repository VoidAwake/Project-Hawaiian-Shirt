using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hawaiian.Input
{
    
    public enum CursorMode
    {
        Static,
        Free
    }
    
    
    public class Cursor : MonoBehaviour
    {
        
        [SerializeField] private Transform _player;
        [SerializeField] private float _baseRadius;
        [SerializeField] private float _maxRadius;
        [SerializeField] private PlayerInput _input;
        
        private PlayerAction action;
        private float _currentRadius;
        private Vector2 _rotation;
        private Vector2 _move;

        public float BaseRadius => _baseRadius;

        public float CurrentRad
        {
            get => _currentRadius;
            set
            {
                if (_currentRadius > _maxRadius)
                {
                    _currentRadius = _maxRadius;
                    return;
                }
                
                _currentRadius = value;
            }
        }

        private Vector3 _keyboardPosition;

      //  public Vector3 GetMousePosition() => _mousePosition;

        public float MaxRadius
        {
            get => _maxRadius;
            set => _maxRadius = value;
        }

        public void OnMove(InputValue value)
        {
            _move = value.Get<Vector2>();
        }

        public void OnRotate(InputValue value)
        {
            _rotation = value.Get<Vector2>();
        }

        public void OnMoveCursor(InputValue value)
        {
            var tempPosition = _keyboardPosition;
            _keyboardPosition = value.Get<Vector2>();

            if (_keyboardPosition == Vector3.zero)
            {
                _keyboardPosition = tempPosition;
            }
            
        }
        private void Awake()
        {
            action = new PlayerAction();
            ResetRadius();
           // action.Player.Rotate.performed += ctx =>_rotation = ctx.ReadValue<Vector2>();
          //  action.Player.Rotate.canceled += ctx => _rotation = Vector2.zero;
        }

        private void OnEnable()
        {
            action.Enable();
        }

        private void OnDisable()
        {
            action.Disable();
        }

        private void Update()
        {

            Vector3 playerInput;
            //var position = new Vector3(_player.transform.position.x, _player.transform.position.y + 0.5f , _player.transform.position.z);

            //if (_input.currentControlScheme == "Gamepad")
            //{
            if (_rotation.magnitude > 0.05f)
            {
                playerInput = _rotation.normalized;
            }
            //else if (_keyboardPosition.magnitude > 0.05f) // _keyboardPosition is never set back to zero you let go of an arrow key, so reticle will never default to movement direction
            //{
            //    playerInput = _keyboardPosition.normalized;
            //}
            else if (_move.magnitude > 0.05f)
            {
                playerInput = _move.normalized;
            }
            else
            {
                Vector3 prevInput = ((transform.localPosition - Vector3.up * 0.5f) / _currentRadius);
                playerInput = prevInput.magnitude == 0 ? Vector2.right.normalized : prevInput.normalized;
            }

            if (transform.parent.localScale.x < 0)
            {
                playerInput = new Vector3(-playerInput.x, playerInput.y, playerInput.z);
            }
            transform.localPosition = playerInput * _currentRadius + Vector3.up * 0.5f;
            //}
            //else
            //{
            //    transform.localPosition = _keyboardPosition * _currentRadius;
            //    // playerInput =  UnityEngine.Input.mousePosition;
            //    //
            //    // Vector3 worldPosition = Camera.main.ScreenToWorldPoint(playerInput);
            //    // // Debug.Log(worldPosition);
            //    // worldPosition.z = 0f; // set to zero since its a 2d game
            //    // var mouseDirection = (worldPosition - position).normalized;
            //    // _mousePosition = position + mouseDirection * _currentRadius;
            //    // // Debug.Log("current radius: " + _currentRadius);
            //    // transform.position = _mousePosition;

            //    // Debug.Log("current transform" + transform.position);
            //}

            // var  playerInput = UnityEngine.Input.mousePosition;
     


            //OLD SYSTEM JUST KEEPING IN CASE
            // Vector2 center = _player.transform.position;
            // Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // float actualDistance = Vector2.Distance(center, position);
            //
            // if (actualDistance > _radius)
            // {
            //     Vector2 centerToPosition = position - center;
            //     centerToPosition.Normalize();
            //     position = center + centerToPosition * _radius;
            //     transform.position = position;
            // }
            // else
            // {
            //     transform.position = position;
            // }


        }

        public Vector3 GetDirectionWithOffset(float offset)
        {
            var position = new Vector3(_player.transform.position.x, _player.transform.position.y , _player.transform.position.z);
            Vector3 playerInput =  UnityEngine.Input.mousePosition;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(playerInput);
            // Debug.Log(worldPosition);
            worldPosition.z = 0f; // set to zero since its a 2d game
            return ((worldPosition - position) * offset);
        }

        public Vector3 GetPositionWithOffset(Vector3 offset)
        {
            Vector3 playerInput;
            var position = new Vector3(_player.transform.position.x, _player.transform.position.y , _player.transform.position.z);
            playerInput =  UnityEngine.Input.mousePosition + offset;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(playerInput);
            // Debug.Log(worldPosition);
            worldPosition.z = 0f; // set to zero since its a 2d game
            var mouseDirection = (worldPosition - position).normalized;
            var mousePosition = position + mouseDirection * _currentRadius;
            // Debug.Log("current radius: " + _currentRadius);
          //  transform.position = mousePosition;
          return mousePosition;
        }


        public void ResetRadius() => _currentRadius = _baseRadius;

        public void LerpToReset()
        {
            StartCoroutine(ResetLerp());
        }

        IEnumerator ResetLerp()
        {
            float waitTime = 0.15f;
            float elapsedTime = 0;

            while (elapsedTime < waitTime)
            {
                _currentRadius = Mathf.Lerp(_currentRadius, _baseRadius, (elapsedTime / waitTime));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
        
    }

}