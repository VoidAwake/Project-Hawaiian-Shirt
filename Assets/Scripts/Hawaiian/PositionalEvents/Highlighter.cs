using System.Collections.Generic;
using Hawaiian.Utilities;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

namespace Hawaiian.PositionalEvents
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Highlighter : MonoBehaviour
    {
        [SerializeField] private Material material;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private SpriteRenderer targetSpriteRenderer;
        
        private List<PositionalEventCaller> callers = new List<PositionalEventCaller>();

        public ReadOnlyArray<PositionalEventCaller> Callers => callers.ToArray();

        // Will not update sprite if an item changes at runtime
        private void Start()
        {
            // Copy all sprite renderer properties
            spriteRenderer.GetCopyOf(targetSpriteRenderer);
            
            spriteRenderer.sortingOrder = targetSpriteRenderer.sortingOrder + 1;
            spriteRenderer.material = material;
            spriteRenderer.enabled = false;
        }

        private void UpdateShader()
        {
            spriteRenderer.material.SetFloat("_NumOfPlayers", callers.Count);

            for (int i = 0; i < callers.Count; i++)
            {
                // TODO: Not sure how to get player colour
                spriteRenderer.material.SetColor("_Color_" + (i + 1), Color.blue);
            }

            spriteRenderer.material.SetTexture("_Texture2D", spriteRenderer.sprite.texture);
            
            spriteRenderer.material.SetVector("_SpritePosition", spriteRenderer.sprite.rect.center);
        }

        public void AddCaller(PositionalEventCaller caller)
        {
            callers.Add(caller);
            
            UpdateShader();

            if (callers.Count > 0)
            {
                spriteRenderer.enabled = true;
            }
        }

        public void RemoveCaller(PositionalEventCaller caller)
        {
            callers.Remove(caller);
            
            UpdateShader();

            if (callers.Count == 0)
            {
                spriteRenderer.enabled = false;
            }
        }
    }
}