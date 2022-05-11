using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Hawaiian.Utilities;
using Hawaiian.Unit;

namespace Hawaiian.UI.MainMenu
{
    public class PauseController : MonoBehaviour
    {
        [SerializeField] ScriptableFloat gameTime;
        MainMenuController menuController;

        public void OnMove(InputValue value)
        {
            if (menuController != null) menuController.OnMove(value);
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
                    menuController = FindObjectOfType<MainMenuController>();
                    if (menuController != null)
                    {
                        if (!menuController.enabled && menuController.pausePlayer == null)
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
            //menuController.gameObject.SetActive(true);
            for (int i = 0; i < menuController.transform.childCount; i++)
            {
                menuController.transform.GetChild(i).gameObject.SetActive(true);
            }

            menuController.enabled = true;
            menuController.pausePlayer = this;
            menuController.CursorToStartingState();

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

            //menuController.gameObject.SetActive(false);
            for (int i = 0; i < menuController.transform.childCount; i++)
            {
                menuController.transform.GetChild(i).gameObject.SetActive(false);
            }

            menuController.enabled = false;
            menuController.pausePlayer = null;
            menuController = null;
        }
    }
}
