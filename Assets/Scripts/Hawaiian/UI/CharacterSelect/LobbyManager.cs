using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


namespace Hawaiian.UI.CharacterSelect
{
    public class LobbyManager : MonoBehaviour
    {
        enum MenuState { CharacterSelect, MainMenu, BoardSelect, MinigameSelect }
        [SerializeField] MenuState menuState;
        [SerializeField] int subState;
        [SerializeField] int buildIndexOfNextScene;
        bool returningToMainMenu;
        LobbyGameManager LobbyGameManager;
        MenuState prevMenuState;
        bool nextReady;
        //bool backReady;

        //[SerializeField] Image transitionCheckers;
        //[SerializeField] Sprite[] transitionCheckersSprites;

        // Character Select Variables
        public int[] playerStickInputs = new int[4];
        public int[] playerButtonInputs = new int[4];

        [Header("Character Select")]
        [SerializeField] LobbyWindow[] windows;
        [SerializeField] CanvasGroup[] portraits;

        [SerializeField] GameObject[] characterSelects;
        [SerializeField] Sprite[] characterSelectSprites;
        [SerializeField] TextMeshProUGUI readyText;
        [SerializeField] GameObject ready;

        int[] playerStatus = new int[4]; // 0 means not in, 1 means loaded in, 2 means loaded in and selected, 3 means a computer player who is selected
        //int firstPlayer;

        // Start is called before the first frame update
        void Start()
        {
            //LobbyGameManager = FindObjectOfType<LobbyGameManager>();
            LobbyGameManager = GetComponent<LobbyGameManager>();

            // Set up character select screen
            for (int i = 0; i < 4; i++)
            {
                playerStatus[i] = 0;
            }

            foreach (LobbyWindow window in windows) window.SetEmpty();
            foreach (GameObject characterSelect in characterSelects) characterSelect.gameObject.SetActive(false);
            ready.SetActive(false);
            //transitionCheckers.enabled = true;
            //transitionCheckers.sprite = transitionCheckersSprites[transitionCheckersSprites.Length - 1];
            //transitionTimer = 0.0f;
            subState = -1;
        }

        // Update is called once per frame
        void Update()
        {
            //Debug.Log(comPlayerNo);

            // Transition to next menu screen
            if (prevMenuState != menuState) HandleTransition();

            // Handle character select screen
            if (menuState == MenuState.CharacterSelect) HandleCharacterSelect();

            // Animate cursors
            for (int i = 0; i < 4; i++)
            {

                if (playerStatus[i] == 1)
                {
                    // Animate cursor
                    characterSelects[i].GetComponent<Image>().sprite = characterSelectSprites
                        [
                            Mathf.FloorToInt((Time.time % 0.39999f) * 10.0f) - (Mathf.FloorToInt((Time.time % 0.39999f) * 10.0f) > 2 ? 2 : 0)
                        ];
                }
                else
                {
                    // Set to still cursor
                    characterSelects[i].GetComponent<Image>().sprite = characterSelectSprites[1];
                }
            }

            // Animate ready text
            if (nextReady)
            {
                readyText.color = new Color(readyText.color.r, readyText.color.g, readyText.color.b, 0.5f + 0.5f * Mathf.Sin(Time.time * 3.0f));
            }
        }

        void HandleTransition()
        {
            // Handle unloading previous state

            // Handle loading next state
            if (menuState == MenuState.CharacterSelect)
            {

            }

            // Set previous state to be current state
            prevMenuState = menuState;
        }

        void HandleCharacterSelect()
        {
            //if (subState == -1 || subState == 1)
            //{
            //    if (transitionTimer > 0.15f)
            //    {
            //        transitionTimer -= 0.15f;
            //        transitionInt++;

            //        if (transitionInt >= transitionCheckersSprites.Length)
            //        {
            //            // Transition out of this... transition.
            //            if (subState == -1)
            //            {
            //                transitionCheckers.enabled = false;
            //                transitionTimer = 0.0f;
            //                transitionInt = 0;
            //                subState++;
            //            }
            //            else
            //            {
            //                if (returningToMainMenu)
            //                {
            //                    SceneManager.LoadScene(0);
            //                }
            //                else
            //                {
            //                    // TODO: Grab player information from this class and LobbyGameManager (which has a PlayerConfig class) and pass it into the next scene

            //                    // Then, load the next scene
            //                    SceneManager.LoadScene(buildIndexOfNextScene);
            //                }
            //            }
            //        }
            //        else
            //        {
            //            // Update the checkers' sprites
            //            if (subState == 1) transitionCheckers.sprite = transitionCheckersSprites[transitionInt];
            //            else transitionCheckers.sprite = transitionCheckersSprites[transitionCheckersSprites.Length - 1 - transitionInt];
            //        }
            //    }

            //    if (subState == -1 || subState == 1) transitionTimer += Time.deltaTime;
            //}
            if (subState == 0)
            {
                // Handle loading in connected players
                if (playerStatus[0] == 0 || playerStatus[1] == 0 || playerStatus[2] == 0 || playerStatus[3] == 0)   // If there aren't 4 players in...
                {
                    foreach (LobbyGameManager.PlayerConfig player in LobbyGameManager.playerConfigs)                              // For each player...
                    {
                        if ((playerStatus[player.playerNumber] == 0) && player.isPlayer)                                    // If they are a person and not yet loaded in, load them in.
                        {
                            int number = player.playerNumber;
                            windows[number].SetUnselected();
                            characterSelects[number].SetActive(true);
                            int count = 0;
                            int nextCharacter = player.characterNumber;
                            while (nextCharacter < 0)
                            {
                                if (CharacterNotChosen(count, true)) nextCharacter = count;
                                count++;
                            }
                            UpdateCharacterSelection(number, nextCharacter);
                            playerStatus[number] = 1;
                            nextReady = false; ready.SetActive(false);
                            playerButtonInputs[number] = 0;
                            playerStickInputs[number] = 0;

                            //// Update text if this is the first player loaded in
                            //count = 0;
                            //for (int i = 0; i < 4; i++) if (playerStatus[i] > 0) count++;
                            //if (count == 1) description.text = "Choose a character to play as.";
                        }
                    }
                }

                // Handle player inputs
                for (int i = 0; i < 4; i++)         // For each player...
                {
                    if (playerStatus[i] == 1)           // If status 1 (loaded in)...
                    {
                        if (playerStickInputs[i] != 0)      // Handle stick movement...
                        {
                            // Find the next available character to select
                            int nextCharacter = (int)Mathf.Repeat(LobbyGameManager.playerConfigs[i].characterNumber + playerStickInputs[i], portraits.Length);
                            while (!CharacterNotChosen(nextCharacter, true))
                            {
                                nextCharacter = (int)Mathf.Repeat(nextCharacter + playerStickInputs[i], portraits.Length);
                            }
                            UpdateCharacterSelection(i, nextCharacter);
                            playerStickInputs[i] = 0;
                        }
                        else if (playerButtonInputs[i] != 0)     // Handle button input...
                        {
                            if (playerButtonInputs[i] == 1)     // Confirm character selection...
                            {
                                windows[i].SetSelected();
                                portraits[LobbyGameManager.playerConfigs[i].characterNumber].alpha = 0.2f;
                                playerStatus[i] = 2;
                                if (AllPlayersSelected(false)) { nextReady = true; ready.SetActive(true); }
                                playerButtonInputs[i] = 0;

                            }
                            if (playerButtonInputs[i] == -1)    // Eject player (deconstruct playerconfig and prefab)...
                            {
                                // Reset visuals for current player to unloaded in versions
                                windows[i].SetEmpty();
                                characterSelects[i].SetActive(false);
                                portraits[LobbyGameManager.playerConfigs[i].characterNumber].alpha = 1.0f;

                                // Unload player
                                playerStatus[i] = 0;
                                LobbyGameManager.playerConfigs[i].manager.UnloadPlayer();
                                playerButtonInputs[i] = 0;

                                //// If no more players in the game, reset text
                                //int count = 0;
                                //for (int j = 0; j < 4; j++) if (playerStatus[i] > 0) count++;
                                //if (count == 1) description.text = "Press any button to join the game.";

                                if (AllPlayersSelected(false))
                                {
                                    nextReady = true;
                                    ready.SetActive(true);
                                }
                            }
                        }
                    }
                    else if (playerStatus[i] == 2)      // If status 2 (selected)...
                    {
                        if (playerButtonInputs[i] == -1)    // Deselect character
                        {
                            windows[i].SetUnselected();
                            portraits[LobbyGameManager.playerConfigs[i].characterNumber].alpha = 1.0f;
                            playerStatus[i] = 1;
                            nextReady = false; ready.SetActive(false);
                            playerButtonInputs[i] = 0;
                            playerStickInputs[i] = 0;
                        }
                        else if (playerButtonInputs[i] == 1)   // Progress to computer player selection OR straight to main menu
                        {
                            if (nextReady)
                            {
                                if (AllPlayersSelected(false)) // Transition to next menu state
                                {
                                    subState = 1; //transitionTimer = 0; transitionInt = 0; transitionCheckers.enabled = true;
                                    FindObjectOfType<Transition>().BeginTransition(true, true, buildIndexOfNextScene, true);
                                }
                                ClearInputs();
                            }
                            playerButtonInputs[i] = 0;
                        }
                    }
                }
            }
        }

        bool CharacterNotChosen(int characterNumber, bool onlyPlayers) // SOMEWHERE, WHEN TRANSITIONING/SETTING UP CHARACTER SELECT, YOU MUST SET characterNumber TO -1 FOR EACH playerConfig WHO !isPlayer // Is this still true?
        {
            foreach (LobbyGameManager.PlayerConfig playerConfig in LobbyGameManager.playerConfigs)
            {
                if (!onlyPlayers || playerConfig.isPlayer) // If only checking human players, and player is not a human, then don't check
                {
                    if (playerConfig.characterNumber == characterNumber) return false;
                }
            }
            return true;
        }

        bool AllPlayersSelected(bool allFour) // Use all four when checking that there are four players (human or not) with characters selected
        {
            int counter = 0;
            foreach (int i in playerStatus)
            {
                if (i == 1) return false;
                if (i == 0)
                {
                    if (allFour) return false;
                    counter++;
                }
            }
            if (counter >= 4) return false;
            return true;
        }

        void ClearInputs()
        {
            for (int i = 0; i < 4; i++)
            {
                playerButtonInputs[i] = 0;
                playerStickInputs[i] = 0;
            }
        }

        void UpdateCharacterSelection(int playerNumber, int charNumber)
        {
            LobbyGameManager.PlayerConfig player = LobbyGameManager.playerConfigs[playerNumber];
            characterSelects[playerNumber].GetComponent<RectTransform>().anchoredPosition = new Vector2(portraits[charNumber].GetComponent<RectTransform>().anchoredPosition.x, portraits[charNumber].GetComponent<RectTransform>().anchoredPosition.y);
            player.characterNumber = charNumber;
            windows[playerNumber].UpdateHead(charNumber);
        }

        public void ReturnToMainMenu()
        {
            int counter = 0;
            int index = -1;
            foreach (int i in playerStatus)
            {
                if (i == 0) counter++;
                else index = i;
            }
            if (counter >= 3)
            {
                if (index >= 0)
                {
                    // Reset visuals for current player to unloaded in versions
                    windows[index].SetEmpty();
                    characterSelects[index].SetActive(false);
                    portraits[LobbyGameManager.playerConfigs[index].characterNumber].alpha = 1.0f;

                    // Unload player
                    playerStatus[index] = 0;
                    LobbyGameManager.playerConfigs[index].manager.UnloadPlayer();
                    playerButtonInputs[index] = 0;
                }

                // Exit this scene
                subState = 1;
                //returningToMainMenu = true;
                //transitionTimer = 0; transitionInt = 0; transitionCheckers.enabled = true;
                FindObjectOfType<Transition>().BeginTransition(true, true, 0, false);
            }
        }

        public void IncrementSubState()
        {
            subState++;
        }
    }
}
