using Hawaiian.Utilities;
using UnityEngine;

namespace Hawaiian.Game
{
    public class GameTimer : MonoBehaviour
    {
        [SerializeField] private float startTime;
        [SerializeField] private ScriptableFloat gameTimeScale;
        [SerializeField] private ScriptableFloat gameTime;
        [SerializeField] private bool startOnAwake;
        [SerializeField] private GameManager gameManager;

        private bool timerActive;

        private void Start()
        {
            gameTime.Value = startTime;
            
            // TODO: Move this to a dedicated script, possibly the game manager
            gameTimeScale.Value = 1;

            gameManager.Phase = GameManager.GamePhase.Stealth;

            if (startOnAwake)
                StartTimer(); 
        }

        private void Update()
        {
            if (!timerActive) return;

            gameTime.Value -= Time.deltaTime * gameTimeScale.Value;

            if (gameTime.Value > 0) return;

            gameTime.Value = 0;

            StopTimer();
            
            // TODO: Move this to a dedicated script, possibly the game manager
            gameTimeScale.Value = 0;
            gameManager.Phase = GameManager.GamePhase.GameOver;

            // TODO: Change to the results screen
        }

        public void StartTimer()
        {
            timerActive = true;
        }
        
        public void StopTimer()
        {
            timerActive = false;
        }
    }
}