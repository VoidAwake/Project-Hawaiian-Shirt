using System;
using UI.Core;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Hawaiian.UI.General
{
    public class OnAnyButtonPress : DialogueComponent<Dialogue>
    {
        public UnityEvent anyButtonPressed = new UnityEvent();

        private IDisposable eventListener;

        protected override void Subscribe()
        {
            eventListener = InputSystem.onAnyButtonPress.CallOnce(ctrl => anyButtonPressed.Invoke());
        }

        protected override void Unsubscribe()
        {
            eventListener.Dispose();
        }
    }
}