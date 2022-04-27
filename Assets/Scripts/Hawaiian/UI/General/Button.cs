using UI.Core;
using UnityEngine.UI;

namespace Hawaiian.UI.General
{
    public abstract class Button<T> : DialogueComponent<T> where T : Dialogue
    {
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

        protected virtual void OnClick() { }
    }
}