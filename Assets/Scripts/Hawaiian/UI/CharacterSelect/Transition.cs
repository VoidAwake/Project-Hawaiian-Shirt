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
        public bool stashPlayerInfo;
        public bool itIsTheCharacterSelectScreenAndYouShouldIncrementTheLobbySubstateOnTransitionConclusion;
        private Image image;
        private float transitionTimer;
        private int transitionInt;

        // Start is called before the first frame update
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

        // Update is called once per frame
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

                            // Do a thing
                            if (itIsTheCharacterSelectScreenAndYouShouldIncrementTheLobbySubstateOnTransitionConclusion)
                            {
                                FindObjectOfType<LobbyManager>().IncrementSubState();
                            }
                        }
                        else
                        {
                            // TODO: Grab player information from this class and LobbyGameManager (which has a PlayerConfig class) and pass it into the next scene
                            if (stashPlayerInfo)
                            {
                                LobbyGameManager manager = FindObjectOfType<LobbyGameManager>();

                                foreach (LobbyGameManager.PlayerConfig config in manager.playerConfigs)
                                {
                                    if (config.IsPlayer) config.SetInputInfo(config.inputComponent);
                                }

                                //Destroy(manager.GetComponent<LobbyManager>());
                                //Destroy(manager.GetComponent<PlayerInputManager>());
                            }

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

        public void BeginTransition(bool fadeOut, bool loadNextScene, int buildIndex, bool stashPlayerInfo)
        {
            transitionActive = true;
            transitionTimer = 0;
            transitionInt = 0;
            image.enabled = true;
            this.fadeOut = fadeOut;
            this.loadNextScene = loadNextScene;
            buildIndexOfNextScene = buildIndex;
            this.stashPlayerInfo = stashPlayerInfo;

            if (fadeOut) image.sprite = transitionCheckersSprites[0];
            else image.sprite = transitionCheckersSprites[transitionCheckersSprites.Length - 1];
        }
    }
}
