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

        private bool timerActive;

        private void Awake()
        {
            gameTime.Value = startTime;
            
            // TODO: Move this to a dedicated script, possibly the game manager
            gameTimeScale.Value = 1;

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