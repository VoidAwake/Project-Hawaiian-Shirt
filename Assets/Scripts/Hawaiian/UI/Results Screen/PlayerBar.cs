using System;
using System.Collections;
using Hawaiian.UI.CharacterSelect;
using TMPro;
using UI.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Hawaiian.UI.Results_Screen
{
    public class PlayerBar : DialogueComponent<ResultsScreenDialogue>
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private Image barImage;
        [SerializeField] private Image playerHeadImage;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private Animator animator;
        [SerializeField] private float spacing;
        [SerializeField] [Range(0, 1)] private float fill;

        public UnityEvent animationCompleted = new UnityEvent();

        private float targetHeight;
        private float score;

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
            playerHeadImage.rectTransform.anchoredPosition = new Vector2(0, fill * targetHeight + spacing);

            barImage.rectTransform.sizeDelta = new Vector2(100, fill * targetHeight);

            scoreText.text = Mathf.CeilToInt(fill * score).ToString();
        }

        private void OnAnimationCompleted()
        {
            animationCompleted.Invoke();
        }

        public void StartAnimation()
        {
            animator.SetTrigger("ShowBar");
        }

        public void Initialise(LobbyGameManager.PlayerConfig player, float maxScore)
        {
            StartCoroutine(InitialiseRoutine(player, maxScore));
        }
        
        private IEnumerator InitialiseRoutine(LobbyGameManager.PlayerConfig player, float maxScore)
        {
            // Wait one frame to allow the RectTransform to update
            yield return null;

            // Height is controlled by the BarChart's Horizontal Layout Group
            var maxHeight = rectTransform.rect.height;

            var maxBarHeight = maxHeight - playerHeadImage.rectTransform.rect.height - spacing;
            
            score = player.score;
            
            targetHeight = score / maxScore * maxBarHeight;

            barImage.color = GetColor(player.playerNumber);
            playerHeadImage.sprite = GetSprite(player.characterNumber);
        }

        // TODO: Duplicate code. See InventoryUI.
        
        [SerializeField] private Sprite[] headSprites;

        public Sprite GetSprite(int characterNo)
        {
            return headSprites[characterNo];
        }
        
        public Color GetColor(int playerNo) {
            switch (playerNo)
            {
                case 0:
                    return new Color(0.764706f, 0.2627451f, 0.3607843f);
                case 1:
                    return new Color(0.2039216f, 0.4f, 0.6901961f);
                case 2:
                    return new Color(0.9450981f, 0.7607844f, 0.3372549f);
                case 3:
                    return new Color(0.5803922f, 0.8000001f, 0.2784314f);
                default:
                    throw new Exception("no");
            }
        }
    }
}