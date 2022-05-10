using UnityEngine;
using TMPro;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

namespace Hawaiian.UI.CharacterSelect
{
    public class LobbyWindow : MonoBehaviour
    {
        public enum CharacterNames { Fox, Robin, Monkey, Cat, Goose, Soup }

        [SerializeField] LobbyGameManager manager;
        [SerializeField] TextMeshProUGUI textNumber;
        [SerializeField] TextMeshProUGUI textPrompt;
        [SerializeField] TextMeshProUGUI textReady;
        [SerializeField] Animator unit;
        [SerializeField] SpriteResolver head;
        [SerializeField] SpriteResolver torso;
        [SerializeField] Camera renderCamera;
        [SerializeField] RawImage renderTexture;

        // Start is called before the first frame update
        void Start()
        {
            SetEmpty();
        }

        // Update is called once per frame
        void Update()
        {
            if (textPrompt.enabled)
            {
                textPrompt.color = new Color(textPrompt.color.r, textPrompt.color.g, textPrompt.color.b, 0.5f + 0.5f * Mathf.Sin(Time.time * 3.0f));
            }
        }

        public void SetEmpty()
        {
            textNumber.enabled = false;
            textPrompt.enabled = true;
            textReady.enabled = false;
            renderCamera.enabled = false;
            renderTexture.enabled = false;

            unit.SetFloat("speed", 0.0f);
            unit.SetBool("isRunning", false);
            unit.speed = 1.0f;
        }

        public void SetUnselected()
        {
            textNumber.enabled = true;
            textPrompt.enabled = false;
            textReady.enabled = false;
            renderCamera.enabled = true;
            renderTexture.enabled = true;

            unit.SetFloat("speed", 0.0f);
            unit.SetBool("isRunning", false);
            unit.speed = 1.0f;
        }

        public void SetSelected()
        {
            textNumber.enabled = true;
            textPrompt.enabled = false;
            textReady.enabled = true;
            renderCamera.enabled = true;
            renderTexture.enabled = true;

            unit.SetFloat("speed", 1.0f);
            unit.SetBool("isRunning", true);
            unit.speed = 5.0f;
        }

        public void UpdateHead(int characterNumber)
        {
            head.SetCategoryAndLabel("Head", ((CharacterNames)characterNumber).ToString());
        }
    }
}
