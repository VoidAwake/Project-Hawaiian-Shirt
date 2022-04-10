using System.Collections.Generic;
using UnityEngine;

namespace Hawaiian.Utilities
{
    [CreateAssetMenu(order = 0, menuName = "Hawaiian/Events/GameEvent", fileName = "NewGameEvent")]
    public class GameEvent : ScriptableObject
    {
        internal List<GameEventListeners> _listeners = new();

        public virtual void Raise()
        {
            for (int i = _listeners.Count - 1; i >= 0; i--)
                _listeners[i].OnEventRaised();
        }
        
        public void RegisterListener(GameEventListeners listener) => _listeners.Add(listener);

        public void UnregisterListener(GameEventListeners listener) => _listeners.Remove(listener);
    }
}
