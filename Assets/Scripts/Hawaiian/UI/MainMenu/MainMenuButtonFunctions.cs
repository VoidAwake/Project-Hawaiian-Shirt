using Hawaiian.Game;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hawaiian.UI.CharacterSelect;
using UnityEngine.Events;

namespace Hawaiian.UI.MainMenu
{
    public class MainMenuButtonFunctions : MonoBehaviour
    {
        enum ButtonFunction { LoadSceneByIndex, CloseApp, DeleteSelf, ResumeGame, DisplayControls }
        [SerializeField] ButtonFunction function;
        [SerializeField] int buildIndex;
        [SerializeField] bool callTransition;

        public UnityEvent displayControlsSelected = new UnityEvent();

        public void LoadSceneByIndex()
        {
            if (callTransition)
            {
                Transition transition = FindObjectOfType<Transition>();
                if (transition != null)
                {
                    transition.BeginTransition(true, true, buildIndex);
                }
                else
                {
                    SceneManager.LoadScene(buildIndex);
                }
            }
            else
            {
                SceneManager.LoadScene(buildIndex);
            }
        }

        public void CloseApp()
        {
            Application.Quit();
        }

        public void DeleteSelf()
        {
            Destroy(gameObject);
        }

        public void ResumeGame()
        {
            FindObjectOfType<MainMenuController>().pausePlayer.ResumeGame();
        }

        public void DisplayControls()
        {
            displayControlsSelected.Invoke();
        }

        public void CallSerializedFunction()
        {
            switch (function)
            {
                case ButtonFunction.LoadSceneByIndex:
                    AudioManager.audioManager.Confirm();
                    LoadSceneByIndex();
                    break;
                case ButtonFunction.CloseApp:
                    CloseApp();
                    break;
                case ButtonFunction.DeleteSelf:
                    DeleteSelf();
                    break;
                case ButtonFunction.ResumeGame:
                    ResumeGame();
                    break;
                case ButtonFunction.DisplayControls:
                    DisplayControls();
                    break;
                default:
                    break;
            }
        }
    }
}
