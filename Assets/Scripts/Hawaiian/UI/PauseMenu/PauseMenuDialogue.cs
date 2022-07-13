using Hawaiian.UI.MainMenu;
using UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Hawaiian.UI.PauseMenu
{
    public class PauseMenuDialogue : Dialogue
    {
        [SerializeField] private Selectable firstSelectable;
        
        protected override void OnClose() { }

        protected override void OnPromote()
        {
            // TODO: For some reason this doesn't trigger MenuButtonCursor.OnSelect
            firstSelectable.Select();
        }

        protected override void OnDemote() { }

        public void ResumeGame()
        {
            PauseController.pausePlayer.ResumeGame();
            
            // TODO: Dialogues shouldn't destroy themselves
            manager.Pop();
        }
    }
}