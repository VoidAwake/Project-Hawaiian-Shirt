using System.Collections;
using Hawaiian.Input;
using Hawaiian.Unit;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hawaiian.Inventory
{
    public class Cursor : MonoBehaviour
    {
        [SerializeField] private float _baseRadius;
        [SerializeField] private float _maxRadius;
        [SerializeField] private UnitPlayer unit;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] PlayerColors playerColors;
        
        private PlayerAction action;
        private float _currentRadius;
        private Vector2 _rotation;
        private Vector2 _move;

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

        public float MaxRadius
        {
            get => _maxRadius;
            set => _maxRadius = value;
        }

        #region Player Input Messages

        public void OnMove(InputValue value)
        {
            _move = value.Get<Vector2>();
        }

        public void OnRotate(InputValue value)
        {
            _rotation = value.Get<Vector2>();
        }

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            action = new PlayerAction();
            ResetRadius();
        }

        private void OnEnable()
        {
            unit.initialised.AddListener(OnInitialised);
            
            OnInitialised();
            
            action.Enable();
        }

        private void OnDisable()
        {
            unit.initialised.RemoveListener(OnInitialised);
            
            action.Disable();
        }

        private void Update()
        {
            Vector3 playerInput;

            if (_rotation.magnitude > 0.05f)
            {
                playerInput = _rotation.normalized;
            }
            else if (_move.magnitude > 0.05f)
            {
                playerInput = _move.normalized;
            }
            else
            {
                Vector3 prevInput = (transform.localPosition - Vector3.up * 0.5f) / _currentRadius;
                playerInput = prevInput.magnitude == 0 ? Vector2.right.normalized : prevInput.normalized;
            }

            if (transform.parent.localScale.x < 0)
            {
                playerInput = new Vector3(-playerInput.x, playerInput.y, playerInput.z);
            }
            
            transform.localPosition = playerInput * _currentRadius + Vector3.up * 0.5f;
        }

        #endregion

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
        
        private void OnInitialised()
        {
            spriteRenderer.color = playerColors.GetColor(unit.PlayerNumber);
        }
    }
}