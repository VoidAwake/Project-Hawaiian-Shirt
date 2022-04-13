using UnityEngine;

namespace Hawaiian.Interactables
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Highlighter : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer targetSpriteRenderer;

        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();

            spriteRenderer.sprite = targetSpriteRenderer.sprite;
            spriteRenderer.sortingOrder = targetSpriteRenderer.sortingOrder - 1;
        }
    }
}