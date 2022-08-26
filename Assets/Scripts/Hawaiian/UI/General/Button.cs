using UI.Core;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Hawaiian.UI.General
{
    public abstract class Button<T> : DialogueComponent<T> where T : Dialogue
    {
        public UnityEvent<Button<T>> clicked = new();
        
        protected Button button;

        protected override void OnComponentAwake()
        {
            TryGetComponent(out button);
        }

        protected override void Subscribe()
        {
            button.onClick.AddListener(OnClick);
        }

        protected override void Unsubscribe()
        {
            button.onClick.RemoveListener(OnClick);
        }

        public virtual void OnClick()
        {
            clicked.Invoke(this);
        }
    }
}