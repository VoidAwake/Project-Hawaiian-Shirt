using UI.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Hawaiian.UI.General
{
    public class MenuButtonCursor : MenuButton<Dialogue>
    {
        [SerializeField] private Image cursor;
        [SerializeField] private Sprite[] cursorSprites;
        
        private bool CursorActive
        {
            get => cursor.enabled;
            set => cursor.enabled = value;
        }
        
        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);

            CursorActive = true;
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);

            CursorActive = false;
        }

        private void Update()
        {
            if (!CursorActive) return;
            
            cursor.sprite = cursorSprites
            [
                Mathf.FloorToInt((Time.time % 0.39999f) * 10.0f) - (Mathf.FloorToInt((Time.time % 0.39999f) * 10.0f) > 2 ? 2 : 0)
            ];
        }
    }
}