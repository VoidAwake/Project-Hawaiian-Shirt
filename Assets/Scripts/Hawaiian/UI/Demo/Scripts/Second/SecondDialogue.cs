using UI.Core;
using UnityEngine;

namespace UI.Demo.Scripts.Second
{
    public class SecondDialogue : Dialogue
    {
        internal readonly Event<bool> toggle = new Event<bool>();
        
        
        protected override void OnClose() => Debug.Log("Dialogue two closed...");

        protected override void OnPromote() => Debug.Log("Dialogue two promoted...");

        protected override void OnDemote() => Debug.Log("Dialogue two demoted...");
    }
}
