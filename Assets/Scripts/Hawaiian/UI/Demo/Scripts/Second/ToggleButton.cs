using UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Demo.Scripts.Second
{
    public class ToggleButton : DialogueComponent<SecondDialogue>
    {
        [SerializeField] private Button button;

        private bool state;
        
        
        protected override void Subscribe() => button.onClick.AddListener(OnClick);

        protected override void Unsubscribe() => button.onClick.RemoveListener(OnClick);

        protected override void OnComponentStart() => OnClick();

        private void OnClick()
        {
            dialogue.toggle.Invoke(state);
            state = !state;
        }
    }
}
