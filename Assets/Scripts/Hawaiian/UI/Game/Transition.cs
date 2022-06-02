using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Hawaiian.Game;
using UnityEngine;
using UnityEngine.UI;

namespace Hawaiian.UI.Game
{
    public class Transition : MonoBehaviour
    {
        [SerializeField] Sprite[] transitionCheckersSprites;
        [SerializeField] float stepDuration;
        [SerializeField] private SceneChanger sceneChanger;
        public bool loadNextScene;
        public bool fadeOut;
        public bool transitionActive;
        private Image image;
        private float transitionTimer;
        private int transitionInt;

        void Start()
        {
            image = GetComponent<Image>();
            if (image == null) Destroy(this);

            if (transitionActive)
            {
                image.enabled = true;
                if (fadeOut) image.sprite = transitionCheckersSprites[0];
                else image.sprite = transitionCheckersSprites[transitionCheckersSprites.Length - 1];
            }
            else image.enabled = false;
        }

        void Update()
        {
            if (transitionActive)
            {
                if (transitionTimer > stepDuration)
                {
                    transitionTimer -= stepDuration;
                    transitionInt++;

                    if (transitionInt >= transitionCheckersSprites.Length)
                    {
                        // Transition out of this... transition.
                        if (!loadNextScene)
                        {
                            image.enabled = false;
                            transitionTimer = 0.0f;
                            transitionInt = 0;
                        }

                        transitionActive = false;
                    }
                    else
                    {
                        // Update the checkers' sprites
                        if (fadeOut) image.sprite = transitionCheckersSprites[transitionInt];
                        else image.sprite = transitionCheckersSprites[transitionCheckersSprites.Length - 1 - transitionInt];
                    }
                }
                transitionTimer += Time.deltaTime;
            }
        }

        public void BeginTransition(bool fadeOut, bool loadNextScene)
        {
            transitionActive = true;
            transitionTimer = 0;
            transitionInt = 0;
            image.enabled = true;
            this.fadeOut = fadeOut;
            this.loadNextScene = loadNextScene;

            if (fadeOut) image.sprite = transitionCheckersSprites[0];
            else image.sprite = transitionCheckersSprites[transitionCheckersSprites.Length - 1];
        }

        private void OnEnable()
        {
            sceneChanger.ChangingScene += OnSceneChanging;
        }

        private void OnDisable()
        {
            sceneChanger.ChangingScene -= OnSceneChanging;
        }

        private async Task OnSceneChanging(object arg1, EventArgs arg2)
        {
            BeginTransition(true, true);

            await UniTask.WaitUntil(() => !transitionActive);
        }
    }
}
