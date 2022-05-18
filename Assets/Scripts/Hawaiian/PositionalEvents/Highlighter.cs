using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

namespace Hawaiian.PositionalEvents
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Highlighter : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        private SpriteRenderer targetSpriteRenderer;
        private List<PositionalEventCaller> callers = new List<PositionalEventCaller>();

        public ReadOnlyArray<PositionalEventCaller> Callers => callers.ToArray();

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            
            targetSpriteRenderer = transform.parent.GetComponent<SpriteRenderer>();

            if (!targetSpriteRenderer)
            {
                enabled = false;
                
                throw new Exception($"Could not highlight. Parent object does not have a {nameof(SpriteRenderer)}");
            }

            spriteRenderer.sprite = targetSpriteRenderer.sprite;
            spriteRenderer.sortingOrder = targetSpriteRenderer.sortingOrder + 1;
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
        }

        public void RemoveCaller(PositionalEventCaller caller)
        {
            callers.Remove(caller);
            
            UpdateShader();
        }
    }
}