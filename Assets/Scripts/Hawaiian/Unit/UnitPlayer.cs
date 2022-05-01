using Hawaiian.Input;
using Hawaiian.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hawaiian.Unit
{
    public class UnitPlayer : Unit, IUnit
    {
        [SerializeField] private GameEvent forward;
        [SerializeField] private GameEvent backward;
        [SerializeField] private GameObject _damageReference;
        [SerializeField] private float _offset;
        [SerializeField] private float _damageIndicatorOffset;
        [SerializeField] private Transform _firePoint;

        [SerializeField] private PlayerAction _input;
        [SerializeField] private PlayerInput _playerInput;

        public PlayerAction GetPlayerAction() => _input;
        public PlayerInput GetPlayerInput() => _playerInput;

       [SerializeField] private PlayerInputManager.PlayerJoinedEvent _playerJoined;

        private bool _attackFlag = false;
        protected override void Awake()
        {
            base.Awake();
            _input = new PlayerAction();
            _playerInput = GetComponent<PlayerInput>();
            Debug.Log(_playerInput.playerIndex + "Current player index");
            //_attackComponent = GetComponent<UnitPlayerAttack>();
        }

        

        public void UpdateCount(PlayerInput input)
        {

            if (_playerInput.playerIndex != input.playerIndex)
                return;
            
            if (input.currentControlScheme == "Gamepad")
            {
                _playerInput.SwitchCurrentControlScheme("Gamepad" + (input.playerIndex == 0 ? "" : input.playerIndex + 1));
            }
            else
            {
                _playerInput.SwitchCurrentControlScheme("Keyboard1" + (input.playerIndex == 0 ? 1 : input.playerIndex + 1));

            }
            
            Debug.Log(_playerInput.currentControlScheme);
        }

        private void OnEnable()
        {
            _input.Enable();
        }

     

        private void OnDisable()
        {
            _input.Disable();
            
         

        }

        protected override void Start()
        {
            base.Start();
            //forward.Raise();
            // _input.Player.Parse.performed += context =>
            // {
            //     if (context.ReadValue<float>() > 0f)
            //     {
            //         forward.Raise();
            //     }
            //     else
            //     {
            //         backward.Raise();
            //     }
            // };


            _input.Player.Rotate.performed += ctx =>  ctx.ReadValue<Vector2>();
            //_input.Player.Rotate.canceled += ctx => //Vector2.zero;
        }

        public void OnMove(InputValue value)
        { 
            move = value.Get<Vector2>();

            // action = new PlayerAction();
            // _input = GetComponent<PlayerInput>();
            // _animator = GetComponent<UnitAnimator>();
            
            // action.Player.Rotate.performed += ctx => UpdateRotation(ctx.ReadValue<Vector2>());
            // action.Player.Rotate.canceled += ctx => CancelRotation();
            //
            // action.Player.HoldAttack.performed += HoldAttack;
            // action.Player.HoldAttack.canceled += HoldAttack;
        }

        protected override void Update()
        {
            base.Update();

            // if (_isHoldingAttack && _cursor.CurrentRad != _cursor.MaxRadius)
            // {
            //     _currentHoldTime += Time.deltaTime;
            //     UpdateHoldAttackCursor();
            // }
        }
        
        // void UpdateRotation(Vector2 newValue)
        // {
        //     _rotation = newValue;
        //     _isJoystickNeutral = false;
        // }
        //
        // void UpdateHoldAttackCursor()
        // {
        //     _cursor.CurrentRad += _currentHoldTime / 10;
        // }
        //
        // void CancelRotation()
        // {
        //     _rotation = Vector2.zero;
        //     _isJoystickNeutral = true;
        // }

        // private void OnEnable()
        // {
        //     action.Enable();
        //     _rotation = Vector2.zero;
        // }
        //
        // private void OnDisable()
        // {
        //     action.Disable();
        // }
        
     //   public void OnMove(InputValue value) => move = value.Get<Vector2>();
        
        //public void OnActionA(InputValue value) { isRunning = value.Get<float>() > 0.1f;} //Debug.Log("Button: " + value.Get<float>()); }

        // public void HoldAttack(InputAction.CallbackContext ctx)
        // {
        //     if (ctx.performed)
        //     {
        //         
        //         _isHoldingAttack = true;
        //         return;
        //     }
        //
        //     var position = _cursor.transform.position;
        //     var instantiatedProjectile = Instantiate(_projectileReference,transform.position,Quaternion.identity);
        //     instantiatedProjectile.GetComponent<Projectile>().Initialise(position);
        //     
        //     _cursor.LerpToReset();
        //     _currentHoldTime = 0f;
        //     _isHoldingAttack = false;
        // }
        //
        //
        // public void OnAttack(InputValue value)
        // {
        //     //NEEDS TO BE A CHECK IF USING PROJECTILES TO ALLOW FOR ON HOLD ACTIONS AND IGNORE THIS
        //     #region Ranged Attack
        //
        //     #endregion
        //
        //     #region MeleeAttack
        //
        //     //  Vector3 playerInput;
        //     //  float angle;
        //     //  
        //     //  var position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        //     // // var mouse = UnityEngine.Input.mousePosition;
        //     //  
        //     //  if (_input.currentControlScheme == "Gamepad")
        //     //  {
        //     //
        //     //      if (_isJoystickNeutral)
        //     //          _rotation = _animator.IsLookingLeft ? Vector2.left : Vector2.right;
        //     //      
        //     //      playerInput = _rotation;
        //     //      _lastAttackPosition = position + (Vector3)_rotation * _offset;
        //     //      angle = Mathf.Atan2(_rotation.y, _rotation.x) * Mathf.Rad2Deg;
        //     //
        //     //  }
        //     //  else
        //     //  {
        //     //      playerInput = UnityEngine.Input.mousePosition;
        //     //      Vector3 worldPosition = Camera.main.ScreenToWorldPoint(playerInput);
        //     //      worldPosition.z = 0f; // set to zero since its a 2d game
        //     //      var mouseDirection = (worldPosition - position).normalized;
        //     //      _lastAttackPosition = position + mouseDirection * _offset;
        //     //      Vector3 difference = Camera.main.ScreenToWorldPoint(playerInput) -position;
        //     //      difference.Normalize();
        //     //      angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        //     //  }
        //     //  
        //     //  
        //     //  _firePoint.position = _lastAttackPosition;
        //     //  GameObject indicator = Instantiate(_damageReference,_lastAttackPosition,Quaternion.Euler(new Vector3(0,0,angle + _damageIndicatorOffset )),_firePoint);
        //     //
        //     //  indicator.GetComponent<DamageIndicator>().Initialise(5,_attackFlag,this);
        //     //  _attackFlag = !_attackFlag;
        //
        //     #endregion
        // }

        public Vector3 GetPosition() =>  transform.position;
        
        
        public void TakeDamage(float damage)
        {
            _health -= damage;
            OnDeath();
        }

        public void OnDeath()
        {
            if (_health > 0)
                return;
            
            Destroy(this.gameObject);
        }
        
        

        public void Use() { }
    }
}
