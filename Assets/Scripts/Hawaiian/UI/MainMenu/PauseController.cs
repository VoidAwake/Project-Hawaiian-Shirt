using UnityEngine;
using UnityEngine.InputSystem;
using Hawaiian.Utilities;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

namespace Hawaiian.UI.MainMenu
{
    public class PauseController : MonoBehaviour
    {
        [SerializeField] ScriptableFloat gameTime;
        [SerializeField] private GameEvent pauseGameEvent;
        [SerializeField] private GameEvent resumeGameEvent;
        [SerializeField] private PlayerInput playerInput;
        
        public static PauseController pausePlayer;
        
        public void OnActionB(InputValue value)
        {
            if (pausePlayer == this)
            {
                ResumeGame();
            }
        }
        
        public void OnPause(InputValue value)
        {
            if (value.Get<float>() < 0.5f) return;

            if (pausePlayer == this)
            {
                ResumeGame();
                return;
            }

            if (pausePlayer != null) return;
                    
            PauseGame();
        }

        public void PauseGame()
        {
            pausePlayer = this;
            
            ((InputSystemUIInputModule) EventSystem.current.currentInputModule).actionsAsset = playerInput.actions;

            gameTime.Value = 0.0f;
            Unit.Unit[] units = FindObjectsOfType<Unit.Unit>();
            foreach (Unit.Unit unit in units)
            {
                unit.enabled = false;
            }

            pauseGameEvent.Raise();
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
            
            resumeGameEvent.Raise();
        }
    }
}
