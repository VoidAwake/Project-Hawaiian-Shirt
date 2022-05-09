using System.Collections;
using System.Collections.Generic;
using UI.Core;
using UnityEngine;
using UnityEngine.Events;

namespace Hawaiian.UI.Results_Screen
{
    public class BarChart : DialogueComponent<ResultsScreenDialogue>
    {
        [SerializeField] private float barAnimationDuration;
        [SerializeField] private List<PlayerBar> bars;

        public UnityEvent animationComplete = new UnityEvent();
        
        protected override void Subscribe() { }

        protected override void Unsubscribe() { }

        public void AnimateBars()
        {
            StartCoroutine(BarAnimation());
        }

        private IEnumerator BarAnimation()
        {
            foreach (var bar in bars)
            {
               bar.AnimateFill(barAnimationDuration);

               yield return new WaitForSeconds(barAnimationDuration);
            }
            
            animationComplete.Invoke();
        }
    }
}