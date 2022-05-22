using System.Globalization;
using TMPro;
using UI.Core;
using UnityEngine;

namespace Hawaiian.UI.Game
{
    public class ScoreUI : DialogueComponent<GameDialogue>
    {
        [SerializeField] private TMP_Text text;
        
        public Inventory.Inventory inventory;

        protected override void OnComponentStart()
        {
            base.OnComponentStart();
            
            inventory.currentItemChanged.AddListener(UpdateText);
            
            UpdateText();
        }

        private void UpdateText()
        {
            text.text = inventory.Score.ToString(CultureInfo.InvariantCulture);
        }

        protected override void Subscribe() { }

        protected override void Unsubscribe() { }
    }
}