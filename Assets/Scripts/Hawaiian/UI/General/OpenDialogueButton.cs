using UI.Core;
using UnityEngine;

namespace Hawaiian.UI.General
{
    public class OpenDialogueButton : Button<Dialogue>
    {
        [SerializeField] private GameObject dialoguePrefab;

        public override void OnClick()
        {
            base.OnClick();
            
            Instantiate(dialoguePrefab, dialogue.transform.parent);
        }
    }
}