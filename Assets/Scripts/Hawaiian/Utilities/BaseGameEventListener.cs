using UnityEngine;
using UnityEngine.Events;

namespace Hawaiian.Utilities
{
    public abstract class BaseGameEventListener<T> : MonoBehaviour
    {
        public BaseGameEvent<T> Event;
        public UnityEvent<T> Response;


        public void UpdateEvent(BaseGameEvent<T> newGameEvent, UnityEvent<T> newResponse)
        {
            Event = newGameEvent;
            Response = newResponse;
            OnEnable();
        }
        
        public void OnEnable()
        {
            if (Response != null)
                Event.RegisterListener(Response);
        }

        internal void OnDisable()
        {
            if (Response != null)
                Event.UnregisterListener(Response);
        }
    }
}