using Hawaiian.UI.MainMenu;
using UI.Core;
using UnityEngine;

namespace Hawaiian.UI.PauseMenu
{
    public class PauseMenuDialogue : Dialogue
    {
        [SerializeField] private MainMenuController mainMenuController;
        
        protected override void OnClose() { }

        protected override void OnPromote()
        {
            mainMenuController.CursorToStartingState();
        }

        protected override void OnDemote() { }

        public void ResumeGame()
        {
            mainMenuController.pausePlayer.ResumeGame();
            
            // TODO: Dialogues shouldn't destroy themselves
            manager.Pop();
        }
    }
}