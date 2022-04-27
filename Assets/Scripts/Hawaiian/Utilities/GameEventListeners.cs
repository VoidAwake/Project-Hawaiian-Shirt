using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Hawaiian.Utilities
{
   public class GameEventListeners : MonoBehaviour
   {

      public GameEvent Event;
      public UnityEvent Response;


      private void OnEnable() => Event.RegisterListener(Response);
      
      private void OnDisable() => Event.UnregisterListener(Response);

      public void OnEventRaised(GameEvent gameEvent) => Response.Invoke();
      
   }
}
