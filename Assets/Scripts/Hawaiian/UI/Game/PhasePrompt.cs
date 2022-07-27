using System;
using System.Collections;
using Hawaiian.Game;
using Hawaiian.Utilities;
using TMPro;
using UI.Core;
using UnityEngine;

namespace Hawaiian.UI.Game
{
    public class PhasePrompt : DialogueComponent<GameDialogue>
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private ScriptableFloat gameTimeScale;
        [SerializeField] private TMP_Text text;
        [SerializeField] private float displayTime;
        [SerializeField] private float transitionTime;
        [SerializeField] private float offset;
        [SerializeField] private GameObject restartButton;

        public GameEvent EndingCinematicCompleted;
        
        private RectTransform rectTransform;
        private Vector3 initialPosition;

        protected override void OnComponentStart()
        {
            base.OnComponentStart();

            rectTransform = GetComponent<RectTransform>();

            initialPosition = rectTransform.anchoredPosition;

            text.text = "";
            
            restartButton.SetActive(false);
        }

        protected override void Subscribe() { }

        protected override void Unsubscribe() { }

        // TODO: Needs to be called some other way
        public void OnPhaseChanged()
        {
            StartCoroutine(ShowPrompt("Time's Up"));
        }

        private IEnumerator ShowPrompt(string promptText)
        {
            // rectTransform.position = initialPosition + new Vector3(-offset, 0);
            
            text.text = promptText;
            gameTimeScale.Value = 0f;
            
            // float transitionTimer = 0;
            //
            // while (transitionTimer < transitionTime)
            // {
            //     transitionTimer += Time.deltaTime;
            //
            //     rectTransform.position = initialPosition + new Vector3((1 - transitionTimer / transitionTime) * -offset, 0);
            // }

            // rectTransform.position = initialPosition;
            yield return new WaitForSeconds(displayTime);
            EndingCinematicCompleted.Raise();
            // transitionTimer = 0;
            //
            // while (transitionTimer < transitionTime)
            // {
            //     transitionTimer += Time.deltaTime;
            //
            //     rectTransform.position = initialPosition + new Vector3((1 - transitionTimer / transitionTime) * offset, 0);
            // }
            
            restartButton.SetActive(true);

            // TODO: This will be the default, but for the gameover screen, there's no point
            // text.text = "";
            // gameTimeScale.Value = 1f;
        } 
    }
}