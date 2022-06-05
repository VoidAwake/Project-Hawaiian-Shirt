using Hawaiian.Input;
using Hawaiian.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hawaiian.Unit
{
    public class UnitPlayer : Unit, IUnit
    {
       // [SerializeField] private PlayerInputManager.PlayerJoinedEvent _playerJoined;
        [SerializeField] private PlayerAction _input;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private GameEvent _playerJoined;
        
        public int GetPlayer { get; set; }


        private bool _attackFlag = false;


        public PlayerAction GetPlayerAction() => _input;
        public PlayerInput GetPlayerInput() => _playerInput;

        
        protected override void Awake()
        {
            base.Awake();
            _input = new PlayerAction();
            _playerInput = GetComponent<PlayerInput>();
            Debug.Log(_playerInput.playerIndex + "Current player index");
           // _playerJoined.Raise();
        }

        public void UpdateCount(PlayerInput input)
        {
            if (_playerInput.playerIndex != input.playerIndex)
                return;
            
            if (input.currentControlScheme == "Gamepad")
                _playerInput.SwitchCurrentControlScheme("Gamepad" + (input.playerIndex == 0 ? "" : input.playerIndex + 1));
            else
                _playerInput.SwitchCurrentControlScheme("Keyboard1" + (input.playerIndex == 0 ? 1 : input.playerIndex + 1));
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
            _input.Player.Rotate.performed += ctx =>  ctx.ReadValue<Vector2>();
        }

        public void OnMove(InputValue value)
        { 
            move = value.Get<Vector2>();
        }


        public int PlayerNumber { get; set; }
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
        
        public void ApplyKnockback(Vector2 direction, float distance)
        {
            ApplyKnockbackOnly(direction, distance);
        }

        public void TripUnit(Vector2 direction, float distance)
        {
           KnockBack(direction,distance);
        }

        public UnitPlayer GetUnit() => this;
        
        public Color32 PlayerColour { get; set; }
    }
}
