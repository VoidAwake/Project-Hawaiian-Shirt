using System;
using Hawaiian.UI.CharacterSelect;
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

        public void Initialise(LobbyGameManager.PlayerConfig player)
        {
            maxHeight = player.score * 10;
            bar.GetComponent<Image>().color = GetColor(player.playerNumber);
            playerImage.GetComponent<Image>().sprite = GetSprite(player.characterNumber);
        }
        
        // TODO: Duplicate code. See InventoryUI.
        
        [SerializeField] private Sprite[] headSprites;

        public Sprite GetSprite(int characterNo)
        {
            // TODO: Can be removed
            if (characterNo == -1) return headSprites[0];
            Debug.Log(characterNo);
            return headSprites[characterNo];
        }
        
        public Color GetColor(int playerNo) {
            switch (playerNo)
            {
                case 0:
                    return new Color(0.764706f, 0.2627451f, 0.3607843f);
                    break;
                case 1:
                    return new Color(0.2039216f, 0.4f, 0.6901961f);
                    break;
                case 2:
                    return new Color(0.9450981f, 0.7607844f, 0.3372549f);
                    break;
                case 3:
                    return new Color(0.5803922f, 0.8000001f, 0.2784314f);
                    break;
                default:
                    throw new Exception("no");
                    break;
            }
        }
    }
}