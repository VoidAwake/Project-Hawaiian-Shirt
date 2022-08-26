using UnityEngine;
using UnityEngine.InputSystem;
using Hawaiian.Utilities;

namespace Hawaiian.UI.MainMenu
{
    public class PauseController : MonoBehaviour
    {
        [SerializeField] ScriptableFloat gameTime;
        [SerializeField] private GameEvent pauseGameEvent;
        
        public static PauseController pausePlayer;
        
        public void OnActionB(InputValue value)
        {
            if (pausePlayer == this)
            {
                ResumeGame();
            }
        }
        public void OnMenu(InputValue value)
        {
            if (!(value.Get<float>() > 0.5f)) return;

            if (pausePlayer == this)
            {
                OnActionB(value);
                return;
            }

            if (pausePlayer != null) return;
            
            pauseGameEvent.Raise();
                    
            PauseGame();
        }

        public void PauseGame()
        {
            pausePlayer = this;

            gameTime.Value = 0.0f;
            Unit.Unit[] units = FindObjectsOfType<Unit.Unit>();
            foreach (Unit.Unit unit in units)
            {
                unit.enabled = false;
            }
        }

        public void ResumeGame()
        {
            gameTime.Value = 1.0f;
            Unit.Unit[] units = FindObjectsOfType<Unit.Unit>();
            foreach (Unit.Unit unit in units)
            {
                unit.enabled = true;
            }

            pausePlayer = null;
        }
    }
}
