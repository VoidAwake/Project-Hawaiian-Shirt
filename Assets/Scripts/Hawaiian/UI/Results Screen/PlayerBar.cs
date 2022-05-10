using Hawaiian.Unit;
using UI.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Hawaiian.UI.Results_Screen
{
    public class PlayerBar : DialogueComponent<ResultsScreenDialogue>
    {
        [SerializeField] private RectTransform bar;
        [SerializeField] private RectTransform playerImage;
        [SerializeField] private Animator animator;
        [SerializeField] private float maxHeight;
        [SerializeField] private float spacing;

        public UnityEvent animationCompleted = new UnityEvent();
        
        [Range(0, 1)] [SerializeField] private float fill;

        protected override void Subscribe() { }

        protected override void Unsubscribe() { }

        private void OnValidate()
        {
            UpdateFill();
        }

        private void Update()
        {
            UpdateFill();
        }

        private void UpdateFill()
        {
            playerImage.anchoredPosition = new Vector2(0, fill * maxHeight + spacing);

            bar.sizeDelta = new Vector2(100, fill * maxHeight);
        }

        private void OnAnimationCompleted()
        {
            animationCompleted.Invoke();
        }

        public void StartAnimation()
        {
            animator.SetTrigger("ShowBar");
        }

        public void Initialise(PlayerData player)
        {
            maxHeight = player.score * 10;
            bar.GetComponent<Image>().color = player.color;
            playerImage.GetComponent<Image>().sprite = player.sprite;
        }
    }
}