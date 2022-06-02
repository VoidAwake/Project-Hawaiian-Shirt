using System.Collections.Generic;
using UnityEngine;

namespace Hawaiian.Utilities
{
    public class SimultaneousInputTester : MonoBehaviour
    {
        [SerializeField] private List<GameObject> gameObjects;
        [SerializeField] private string methodName;

        [ContextMenu("SendMessages")]
        private void SendMessages()
        {
            foreach (var gameObject in gameObjects)
            {
                gameObject.BroadcastMessage(methodName);
            }
        }
    }
}