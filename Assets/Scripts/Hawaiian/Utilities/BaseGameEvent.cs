using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Hawaiian.Utilities
{
    // https://forum.unity.com/threads/generic-scriptable-object-events.1148393/
    public abstract class BaseGameEvent<T> : ScriptableObject
    {
        private List<UnityEvent<T>> _listeners = new List<UnityEvent<T>>();

        public void Raise(T t)
        {
            foreach (var listener in _listeners.ToList())
            {
                listener.Invoke(t);
            }
        }
        
        public void RegisterListener(UnityEvent<T> listener) => _listeners.Add(listener);

        public void UnregisterListener(UnityEvent<T> listener) => _listeners.Remove(listener);
    }
}