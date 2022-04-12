using UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Demo.Scripts
{
    public class OpenDialogueButton : DialogueComponent<Dialogue>
    {
        [SerializeField] private GameObject dialoguePrefab;
        [SerializeField] private Button button;


        protected override void Subscribe() => button.onClick.AddListener(OnPressed);

        protected override void Unsubscribe() => button.onClick.RemoveListener(OnPressed);
        
        private void OnPressed() => Instantiate(dialoguePrefab, dialogue.transform.parent);
    }
}
