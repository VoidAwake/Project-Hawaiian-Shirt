using UnityEngine;
using UnityEngine.Events;

namespace Hawaiian.Utilities
{
   public class GameEventListener : MonoBehaviour
   {
      public GameEvent Event;
      public UnityEvent Response;

      private void OnEnable() => Event.RegisterListener(Response);

      private void OnDisable() => Event.UnregisterListener(Response);
   }
}
