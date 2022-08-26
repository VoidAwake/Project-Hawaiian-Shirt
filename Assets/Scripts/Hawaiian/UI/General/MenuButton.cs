using UI.Core;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Hawaiian.UI.General
{
    public abstract class MenuButton<T> : Button<T>, ISelectHandler, IDeselectHandler where T : Dialogue
    {
        public UnityEvent<MenuButton<T>> selected = new();
        public UnityEvent<MenuButton<T>> deselected = new();
        
        public bool IsSelected { get; private set; }

        public virtual void OnSelect(BaseEventData eventData)
        {
            IsSelected = true;
            selected.Invoke(this);
        }

        public virtual void OnDeselect(BaseEventData eventData)
        {
            IsSelected = false;
            deselected.Invoke(this);
        }
    }
}