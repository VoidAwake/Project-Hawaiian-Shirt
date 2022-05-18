using UnityEngine;
using UnityEngine.InputSystem;

namespace Hawaiian.UI.CharacterSelect
{
    public class LobbyPlayerController : MonoBehaviour
    {
        int actionA = 0;    // INPUT STATUSES:
        int actionB = 0;    // 0 - Up
        int actionL = 0;    // 1 - Down, Action Buffered
        int actionR = 0;    // 2 - Down, Action Recieved
        private float move = 0;

        public void OnCharacterSelect(InputValue value) { move = value.Get<float>(); }
        public void OnActionA(InputValue value) { HandleButtonInput(value.Get<float>(), ref actionA); }
        public void OnActionB(InputValue value) { HandleButtonInput(value.Get<float>(), ref actionB); }
        public void OnActionL(InputValue value) { HandleButtonInput(value.Get<float>(), ref actionL); }
        public void OnActionR(InputValue value) { HandleButtonInput(value.Get<float>(), ref actionR); }

        void HandleButtonInput(float value, ref int status)
        {
            if (status == 0 && value > 0.55f) status++;         // If previously up (0) but now down, buffer an action (1)
            else if (status == 2 && value < 0.45f) status = 0;  // If previous down and already used (2) but now up, reset to being up (0)
        }

        int moveBuffer;
        LobbyManager mainMenuManager;
        LobbyGameManager gameManager;
        LobbyGameManager.PlayerConfig playerConfig;

        float selfDestructTimer;
        float inputBirthTimer;

        // Start is called before the first frame update
        void Start()
        {
            mainMenuManager = FindObjectOfType<LobbyManager>();
            gameManager = FindObjectOfType<LobbyGameManager>();
            if (gameManager != null)
            {
                playerConfig = gameManager.AddPlayer(this);
            }

            inputBirthTimer = 0.1f;
            moveBuffer = 1;
        }

        // Update is called once per frame
        void Update()
        {
            if (inputBirthTimer <= 0.0f)
            {
                if (playerConfig != null)
                {
                    if (moveBuffer != 0) // Reset stick input
                    {
                        //if (moveBuffer > 0 && move.x < 0.1f) moveBuffer = 0;
                        //if (moveBuffer < 0 && move.x > -0.1f) moveBuffer = 0;
                        if (move > -0.1f && move < 0.1f) moveBuffer = 0;
                    }
                    else //  Send stick input
                    {
                        if (move > 0.15f) { mainMenuManager.playerStickInputs[playerConfig.playerNumber] = 1; moveBuffer = 1; }
                        if (move < -0.15f) { mainMenuManager.playerStickInputs[playerConfig.playerNumber] = -1; moveBuffer = -1; }
                    }
                    if (actionA == 1) { mainMenuManager.playerButtonInputs[playerConfig.playerNumber] = 1; actionA++; }
                    if (actionB == 1) { mainMenuManager.playerButtonInputs[playerConfig.playerNumber] = -1; actionB++; }
                }
            }
            else
            {
                inputBirthTimer -= Time.deltaTime;

                // Send exit signal to lobby manager
                if (actionB > 0) mainMenuManager.ReturnToMainMenu();

                // Exit birth state, zero inputs
                if (inputBirthTimer < 0.0f)
                {
                    inputBirthTimer = 0.0f;
                    actionA = 0;
                    actionB = 0;
                }
            }

            if (playerConfig == null)
            {
                selfDestructTimer += Time.deltaTime;
                if (selfDestructTimer > 0.5f) Destroy(gameObject);
            }
        }

        public void SetPlayerConfig(LobbyGameManager.PlayerConfig playerConfig)
        {
            this.playerConfig = playerConfig;
        }

        public void UnloadPlayer()
        {
            if (playerConfig != null) playerConfig.Clear();
            playerConfig = null;
            selfDestructTimer = 0.0f;
        }
    }
}
