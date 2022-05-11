using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hawaiian.PositionalEvents
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Highlighter : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        private SpriteRenderer targetSpriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            targetSpriteRenderer = transform.parent.GetComponent<SpriteRenderer>();

            if (!targetSpriteRenderer)
                throw new Exception($"Could not highlight. Parent object does not have a {nameof(SpriteRenderer)}");

            spriteRenderer.sprite = targetSpriteRenderer.sprite;
            spriteRenderer.sortingOrder = targetSpriteRenderer.sortingOrder + 1;
        }

        public void Initialise(int numOfPlayers, List<Color> colours)
        {
            spriteRenderer.material.SetFloat("_NumOfPlayers", numOfPlayers);

            for (int i = 0; i < colours.Count; i++)
            {
                spriteRenderer.material.SetColor("_Color_" + (i + 1), colours[i]);
            }

            Debug.Log(spriteRenderer.sprite.textureRect.center);
            
            spriteRenderer.material.SetTexture("_Texture2D", spriteRenderer.sprite.texture);
            
            spriteRenderer.material.SetVector("_SpritePosition", spriteRenderer.sprite.rect.center);
        }
    }
}