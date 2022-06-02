using Hawaiian.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hawaiian.Game
{
    public class GameTimer : MonoBehaviour
    {
        [SerializeField] private float startTime;
        [SerializeField] private ScriptableFloat gameTimeScale;
        [SerializeField] private ScriptableFloat gameTime;
        [SerializeField] private bool startOnAwake;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private InputAction debugEndGameAction;
       
        private bool timerActive;

        private void Start()
        {
            gameTime.Value = startTime;
            
            if (startOnAwake)
                StartTimer(); 
        }

        private void Update()
        {
            if (!timerActive) return;

            gameTime.Value -= Time.deltaTime * gameTimeScale.Value;

            if (gameTime.Value > 0) return;

            TimesUp();
        }

        public void StartTimer()
        {
            timerActive = true;
        }
        
        public void StopTimer()
        {
            timerActive = false;
        }

        [ContextMenu("TimesUp")]
        private void TimesUp()
        {
            gameTime.Value = 0;

            StopTimer();
            
            // TODO: Move this to a dedicated script, possibly the game manager
            gameTimeScale.Value = 0;
            gameManager.Phase = GameManager.GamePhase.GameOver;

            // TODO: Change to the results screen
        }

        private void OnEnable()
        {
            debugEndGameAction.Enable();
            
            debugEndGameAction.performed += OnDebugEndGameActionPerformed;
        }

        private void OnDisable()
        {
            debugEndGameAction.Disable();
            
            debugEndGameAction.performed -= OnDebugEndGameActionPerformed;
        }

        private void OnDebugEndGameActionPerformed(InputAction.CallbackContext callbackContext)
        {
            TimesUp();
        }
    }
}