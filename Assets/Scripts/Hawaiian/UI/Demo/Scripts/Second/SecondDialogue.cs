using UI.Core;
using UnityEngine;
using UnityEngine.Events;

namespace Hawaiian.UI.Demo.Scripts.Second
{
    public class SecondDialogue : Dialogue
    {
        internal readonly UnityEvent<bool> toggle = new UnityEvent<bool>();
        
        
        protected override void OnClose() => Debug.Log("Dialogue two closed...");

        protected override void OnPromote() => Debug.Log("Dialogue two promoted...");

        protected override void OnDemote() => Debug.Log("Dialogue two demoted...");
    }
}
