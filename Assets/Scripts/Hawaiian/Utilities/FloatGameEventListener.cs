using UnityEngine.Events;

namespace Hawaiian.Utilities
{
    public class FloatGameEventListener : BaseGameEventListener<float>
    {
        public void UpdateEvent(FloatGameEvent newGameEvent, UnityEvent<float> processParry)
        {
            Event = newGameEvent;
            Response = processParry;
            OnEnable();
        }
    }
}
