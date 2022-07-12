using UnityEngine;
using UnityEngine.InputSystem;
using Hawaiian.Utilities;

namespace Hawaiian.UI.MainMenu
{
    public class PauseController : MonoBehaviour
    {
        [SerializeField] ScriptableFloat gameTime;
        [SerializeField] private GameEvent pauseGameEvent;
        
        private MainMenuController menuController;

        public void OnMenuSelect(InputValue value)
        {
            if (menuController != null) menuController.OnMenuSelect(value);
        }
        public void OnActionA(InputValue value)
        {
            if (menuController != null) menuController.OnActionA(value);
        }
        public void OnActionB(InputValue value)
        {
            if (menuController != null) menuController.OnActionB(value);
        }
        public void OnMenu(InputValue value)
        {
            if (menuController == null)
            {
                if (value.Get<float>() > 0.5f)
                {
                    pauseGameEvent.Raise();
                    
                    menuController = FindObjectOfType<MainMenuController>();
                    
                    if (menuController != null)
                    {
                        if (menuController.pausePlayer == null)
                        {
                            PauseGame();
                        }
                        else
                        {
                            menuController = null;
                        }
                    }
                }
            }
            else
            {
                menuController.OnActionB(value);
            }
        }

        public void PauseGame()
        {
            menuController.pausePlayer = this;

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

            menuController.pausePlayer = null;
            menuController = null;
        }
    }
}
