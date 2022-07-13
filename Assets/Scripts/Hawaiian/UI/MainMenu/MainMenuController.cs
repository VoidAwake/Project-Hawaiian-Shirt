using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace Hawaiian.UI.MainMenu
{
    // TODO: This class needs refactoring
    public class MainMenuController : MonoBehaviour
    {
        private void OnEnable()
        {
            buttons = GetComponentsInChildren<Button>();
        }

        public void OnMenuSelect(InputValue value)
        {
            var move = value.Get<float>();

            // TODO: Magic number
            if (Mathf.Abs(move) < 0.15f) return;

            var direction = move > 0 ? 1 : -1;

            if (CheckCursorActive()) return;

            selected = (int)Mathf.Repeat(selected + direction, buttons.Length);
            AudioManager.audioManager.Swap();
            cursor.rectTransform.anchoredPosition =
                buttons[selected].GetComponent<RectTransform>().anchoredPosition;
            buttons[selected].Select();
        }

        public void OnActionA(InputValue value)
        {
            if (CheckCursorActive()) return;
            
            buttons[selected].onClick.Invoke();
        }

        public void OnActionB(InputValue value)
        {
            if (CheckCursorActive()) return;
            
            if (pausePlayer != null)
            {
                pausePlayer.ResumeGame();
            }
        }

        private bool CheckCursorActive()
        {
            if (!cursorActive)
            {
                cursor.enabled = true;
                cursorActive = true;

                return true;
            }

            return false;
        }

        // TODO: Cursor should be moved to another class
        private Button[] buttons;
        public Image cursor;
        [SerializeField] Sprite[] cursorSprites;
        bool cursorActive;
        int selected;

        public PauseController pausePlayer;

        void Start()
        {
            cursorActive = false;
            cursor.enabled = false;
        }

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
        }

        public void CursorToStartingState()
        {
            selected = 0;
            cursor.rectTransform.anchoredPosition = buttons[selected].GetComponent<RectTransform>().anchoredPosition;
            cursor.enabled = false;
            cursorActive = false;
        }
    }
}
