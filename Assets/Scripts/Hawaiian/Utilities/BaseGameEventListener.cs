using UnityEngine;
using UnityEngine.Events;

namespace Hawaiian.Utilities
{
    public abstract class BaseGameEventListener<T> : MonoBehaviour
    {
      public BaseGameEvent<T> Event;
      public UnityEvent<T> Response;

      private void OnEnable() => Event.RegisterListener(Response);

      private void OnDisable() => Event.UnregisterListener(Response);
    }
}