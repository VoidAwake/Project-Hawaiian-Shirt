using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Hawaiian.Utilities
{
    [CreateAssetMenu(order = 0, menuName = "Hawaiian/Events/GameEvent")]
    public class GameEvent : ScriptableObject
    {
        protected internal List<UnityEvent> _listeners = new List<UnityEvent>();

        public virtual void Raise()
        {
            // TODO: Does this prevent loop failure due to array modification? 
            for (int i = _listeners.Count - 1; i >= 0; i--)
                _listeners[i].Invoke();
        }
        
        public virtual void RegisterListener(UnityEvent listener) => _listeners.Add(listener);

        public virtual void UnregisterListener(UnityEvent listener) => _listeners.Remove(listener);
    }
}
