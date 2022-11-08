using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

namespace Hawaiian.PositionalEvents
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Highlighter : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        private List<PositionalEventCaller> callers = new List<PositionalEventCaller>();

        public ReadOnlyArray<PositionalEventCaller> Callers => callers.ToArray();

        private void UpdateShader()
        {
            spriteRenderer.material.SetFloat("_NumOfPlayers", callers.Count);

            for (int i = 0; i < callers.Count; i++)
            {
                // TODO: Not sure how to get player colour
                spriteRenderer.material.SetColor("_Color_" + (i + 1), Color.blue);
            }
        }

        public void AddCaller(PositionalEventCaller caller)
        {
            callers.Add(caller);
            
            UpdateShader();
        }

        public void RemoveCaller(PositionalEventCaller caller)
        {
            callers.Remove(caller);
            
            UpdateShader();
        }
    }
}