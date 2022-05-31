using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Hawaiian.UI.CharacterSelect
{
    public class Transition : MonoBehaviour
    {
        [SerializeField] Sprite[] transitionCheckersSprites;
        [SerializeField] float stepDuration;
        public int buildIndexOfNextScene;
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
                        else
                        {
                            // Destroy persistent gameobject if going to main menu or character select scenes
                            if (buildIndexOfNextScene <= 1)
                            {
                                LobbyGameManager[] lobbyGameManagers = FindObjectsOfType<LobbyGameManager>();
                                foreach (LobbyGameManager lobbyGameManager in lobbyGameManagers)
                                {
                                    Destroy(lobbyGameManager.gameObject);
                                    Debug.Log("I killed the game manager. There can only be one.");
                                }
                            }

                            // Then, load the next scene
                            SceneManager.LoadScene(buildIndexOfNextScene);
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

        public void BeginTransition(bool fadeOut, bool loadNextScene, int buildIndex)
        {
            transitionActive = true;
            transitionTimer = 0;
            transitionInt = 0;
            image.enabled = true;
            this.fadeOut = fadeOut;
            this.loadNextScene = loadNextScene;
            buildIndexOfNextScene = buildIndex;

            if (fadeOut) image.sprite = transitionCheckersSprites[0];
            else image.sprite = transitionCheckersSprites[transitionCheckersSprites.Length - 1];
        }
    }
}
