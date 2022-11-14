using TMPro;
using UI.Core;
using UnityEngine;
using System.Collections;
using System.Linq;
using Hawaiian.Inventory;

namespace Hawaiian.UI.Game
{
    public class ScoreUI : DialogueComponent<GameDialogue>
    {
        [SerializeField] private TMP_Text text;

        Coroutine textCoroutine;
        int currentScore;
        int targetScore;
        float fontSize;
        // float timer;

        // TODO: Where is this getting set?
        public Score score;

        protected override void OnComponentStart()
        {
            base.OnComponentStart();
            
            score.scoreChanged.AddListener(UpdateText);

            fontSize = text.fontSize;
            UpdateText();
        }

        private void UpdateText()
        {
            targetScore = score.ScoreValue;

            if (textCoroutine != null)
            {
                StopCoroutine(textCoroutine);
            }
            textCoroutine = StartCoroutine(AnimateText());
        }

        private IEnumerator AnimateText()
        {
            bool metTarget = false;
            int rate = 0;

            if (currentScore < targetScore)
            {
                rate = 1;
                //text.rectTransform.localScale = new Vector2(0.85f, 1.0f);
                text.fontSize = fontSize * 1.2f;
            }
            else if (targetScore < currentScore)
            {
                rate = -1;
                //text.rectTransform.localScale = new Vector2(1.0f, 0.65f);
                text.fontSize = fontSize * 0.8f;
            }

            while (!metTarget)
            {
                currentScore += rate;
                text.text = "" + currentScore;

                if (currentScore != targetScore)
                {
                    yield return new WaitForSeconds(0.02f);
                }
                else
                {
                    metTarget = true;
                }
            }

            //text.rectTransform.localScale = new Vector2(1,1);
            text.fontSize = fontSize;
            textCoroutine = null;
        }

        protected override void Subscribe() { }

        protected override void Unsubscribe() { }
    }
}