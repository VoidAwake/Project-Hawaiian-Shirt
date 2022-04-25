using System.Linq;
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
            
            inventory.itemchange.AddListener(UpdateText);
            
            UpdateText();
        }

        private void UpdateText()
        {
            text.text = "Score: " + inventory.inv.Where(i => i != null).Sum(i => i.score);
        }

        protected override void Subscribe() { }

        protected override void Unsubscribe() { }
    }
}