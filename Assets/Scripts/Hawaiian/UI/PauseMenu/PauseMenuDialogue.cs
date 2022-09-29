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

        protected override void OnPromote() { }

        protected override void OnDemote() { }

        protected override void Start()
        {
            base.Start();
            
            // Has to be called in start because ISelectHandlers are registered in Awake
            firstSelectable.Select();
        }

        public void ResumeGame()
        {
            // TODO: Dialogues shouldn't destroy themselves
            manager.Pop();
        }
    }
}