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
        [SerializeField] bool callTransition;
        [SerializeField] private SceneChanger sceneChanger;
        // TODO: UI should not be handling scene references
        [SerializeField] private SceneReference scene;

        public UnityEvent displayControlsSelected = new UnityEvent();

        public void LoadSceneByIndex()
        {
            // TODO: Need to account for this is SceneChanger
            if (callTransition)
            {
                sceneChanger.ChangeScene(scene);
            }
            else
            {
                SceneManager.LoadScene(scene);
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
