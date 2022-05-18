using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace Hawaiian.UI.MainMenu
{
    public class MainMenuController : MonoBehaviour
    {
        int actionA = 0;    // INPUT STATUSES:
        int actionB = 0;    // 0 - Up
        private float move = 0;

        public void OnMenuSelect(InputValue value) { move = value.Get<float>(); }
        public void OnActionA(InputValue value) { HandleButtonInput(value.Get<float>(), ref actionA); }
        public void OnActionB(InputValue value) { HandleButtonInput(value.Get<float>(), ref actionB); }

        void HandleButtonInput(float value, ref int status)
        {
            if (status == 0 && value > 0.55f) status++;         // If previously up (0) but now down, buffer an action (1)
            else if (status == 2 && value < 0.45f) status = 0;  // If previous down and already used (2) but now up, reset to being up (0)
        }

        int moveBuffer;

        [SerializeField] Button[] buttons;
        [SerializeField] Image cursor;
        [SerializeField] Sprite[] cursorSprites;
        bool cursorActive;
        int selected;
        public bool disabled;

        public PauseController pausePlayer;

        // Start is called before the first frame update
        void Start()
        {
            cursorActive = false;
            cursor.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            // Animate cursor
            if (cursorActive)
            {
                cursor.sprite = cursorSprites
                    [
                        Mathf.FloorToInt((Time.time % 0.39999f) * 10.0f) - (Mathf.FloorToInt((Time.time % 0.39999f) * 10.0f) > 2 ? 2 : 0)
                    ];
            }

            if (!disabled)
            {
                // Handle input
                if (moveBuffer != 0) // Reset stick input
                {
                    //if (moveBuffer > 0 && move.x < 0.1f) moveBuffer = 0;
                    //if (moveBuffer < 0 && move.x > -0.1f) moveBuffer = 0;
                    if (move > -0.1f && move < 0.1f) moveBuffer = 0;
                }
                else //  Send stick input
                {
                    if (move > 0.15f)
                    {
                        if (!cursorActive) { cursor.enabled = true; cursorActive = true; }
                        else
                        {
                            selected = (selected - 1) % (buttons.Length);
                            if (selected < 0) selected = buttons.Length + selected;
                            cursor.rectTransform.anchoredPosition = buttons[selected].GetComponent<RectTransform>().anchoredPosition;
                            //Debug.Log(selected + " " + buttons.Length);
                        }
                        moveBuffer = 1;
                    }
                    if (move < -0.15f)
                    {
                        if (!cursorActive) { cursor.enabled = true; cursorActive = true; }
                        else
                        {
                            selected = (selected + 1) % (buttons.Length);
                            cursor.rectTransform.anchoredPosition = buttons[selected].GetComponent<RectTransform>().anchoredPosition;
                            //Debug.Log(selected +" "+ buttons.Length);
                        }
                        moveBuffer = -1;
                    }
                }
                if (actionA == 1)
                {
                    if (!cursorActive) { cursor.enabled = true; cursorActive = true; }
                    else
                    {
                        buttons[selected].GetComponent<MainMenuButtonFunctions>().CallSerializedFunction();
                        disabled = true;
                    }
                    actionA++;
                }
                if (actionB == 1)
                {
                    if (!cursorActive) { cursor.enabled = true; cursorActive = true; }
                    else
                    {
                        if (pausePlayer != null)
                        {
                            pausePlayer.ResumeGame();
                        }
                    }
                    actionB++;
                }
            }
        }

        public void CursorToStartingState()
        {
            selected = 0;
            cursor.rectTransform.anchoredPosition = buttons[selected].GetComponent<RectTransform>().anchoredPosition;
            cursor.enabled = false;
            cursorActive = false;
            disabled = false;
        }

        public void EnableCursor()
        {
            disabled = false;
        }
    }
}