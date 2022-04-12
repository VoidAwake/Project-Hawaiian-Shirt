using Audio.Commands;
using Commands;
using Managers;
using UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Demo.Scripts
{
    public class CloseDialogueButton : DialogueComponent<Dialogue>
    {
        [SerializeField] private Button button;

        
        protected override void Subscribe() => button.onClick.AddListener(OnClick);

        protected override void Unsubscribe() => button.onClick.RemoveListener(OnClick);

        private void OnClick()
        {
            CommandManager commandManager = ManagerLocator.Get<CommandManager>();
            commandManager.ExecuteCommand(new PostSound("Stop_Credits_Theme"));
            commandManager.ExecuteCommand(new PostSound("Play_Opening_Theme"));
            
            manager.Pop();
        }
    }
}
