using System;
using System.Collections;
using UI.Core;
using UnityEngine;

namespace Hawaiian.UI.Results_Screen
{
    public class PlayerBar : DialogueComponent<ResultsScreenDialogue>
    {
        [SerializeField] private RectTransform bar;
        [SerializeField] private RectTransform playerImage;
        [SerializeField] private float maxHeight;
        
        [Range(0, 1)]
        public float fill;
        
        protected override void Subscribe() { }

        protected override void Unsubscribe() { }

        private void OnValidate()
        {
            UpdateFill();
        }

        private void UpdateFill()
        {
            playerImage.anchoredPosition = new Vector2(0, fill * maxHeight);

            bar.sizeDelta = new Vector2(100, fill * maxHeight);
        }

        public void AnimateFill(float duration)
        {
            StartCoroutine(FillAnimation(duration));
        }

        private IEnumerator FillAnimation(float duration)
        {
            float timer = 0;

            while (timer < duration)
            {
                fill = timer / duration;
                
                UpdateFill();

                timer += Time.deltaTime;

                yield return null;
            }

            fill = 1;
        }
    }
}