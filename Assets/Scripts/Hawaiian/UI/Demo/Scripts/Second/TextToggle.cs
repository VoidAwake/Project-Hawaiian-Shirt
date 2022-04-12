using TMPro;
using UI.Core;
using UnityEngine;

namespace UI.Demo.Scripts.Second
{
    public class TextToggle : DialogueComponent<SecondDialogue>
    {
        [SerializeField] private TextMeshProUGUI text;
        
        
        protected override void Subscribe() => dialogue.toggle.AddListener(OnToggle);

        protected override void Unsubscribe() => dialogue.toggle.RemoveListener(OnToggle);

        private void OnToggle(bool state) => text.text = state ? "On" : "Off";
    }
}
