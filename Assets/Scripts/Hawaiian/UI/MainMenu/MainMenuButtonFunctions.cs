using Hawaiian.Game;
using Hawaiian.UI.CharacterSelect;
using UnityEngine;
using UnityEngine.Events;

namespace Hawaiian.UI.MainMenu
{
    // TODO: I hate this class.
    public class MainMenuButtonFunctions : MonoBehaviour
    {
        enum ButtonFunction { LoadSceneByIndex, CloseApp, DeleteSelf, ResumeGame, DisplayControls }
        [SerializeField] ButtonFunction function;
        [SerializeField] private SceneChanger sceneChanger;
        // TODO: UI should not be handling scene references
        [SerializeField] private SceneReference scene;
        public bool isModeSelectButton; // Cool code, for mode select screen

        public GameModeSO gameMode; // Even cooler code for mode select
        
        public UnityEvent displayControlsSelected = new UnityEvent();

        public void LoadSceneByIndex()
        {
            // TODO: This needs another look.
            if (isModeSelectButton)
            {
                var lobby = FindObjectOfType<SuperLobbyManager>();
                if (lobby != null)
                {
                    lobby.SetGameMode(gameMode);
                    lobby.StartGame();
                }
            }
            else
            {
                sceneChanger.ChangeScene(scene);
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
        
        public void SetButtonFunction(int newFunction, GameModeSO currentGameMode)
        {
            function = (ButtonFunction)newFunction;
            gameMode = currentGameMode;
        }
    }
}
