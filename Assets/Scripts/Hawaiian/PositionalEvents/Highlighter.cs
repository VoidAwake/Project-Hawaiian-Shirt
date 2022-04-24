using System;
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
            spriteRenderer.sortingOrder = targetSpriteRenderer.sortingOrder - 1;
        }
    }
}