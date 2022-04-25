using UI.Core;
using UnityEngine;

namespace Hawaiian.UI.Demo.Scripts.First
{
    public class FirstDialogue : Dialogue
    {
        protected override void OnClose() => Debug.Log("Dialogue one closed...");

        protected override void OnPromote() => Debug.Log("Dialogue one promoted...");

        protected override void OnDemote() => Debug.Log("Dialogue one demoted...");
    }
}
