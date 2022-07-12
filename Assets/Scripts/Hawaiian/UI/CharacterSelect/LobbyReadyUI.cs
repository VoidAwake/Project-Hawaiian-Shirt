using TMPro;
using UnityEngine;

namespace Hawaiian.UI.CharacterSelect
{
    public class LobbyReadyUI : MonoBehaviour
    {
        [SerializeField] private LobbyManager lobbyManager;
        [SerializeField] private GameObject display;
        [SerializeField] private TextMeshProUGUI readyText;

        private void OnEnable()
        {
            lobbyManager.readyChanged.AddListener(OnReadyChanged);
            
            OnReadyChanged();
        }
        
        private void OnDisable()
        {
            lobbyManager.readyChanged.RemoveListener(OnReadyChanged);
        }

        private void Update()
        {
            if (lobbyManager.ReadyToStartGame)
            {
                readyText.color = new Color(readyText.color.r, readyText.color.g, readyText.color.b, 0.5f + 0.5f * Mathf.Sin(Time.time * 3.0f));
            }
        }

        private void OnReadyChanged()
        {
            display.SetActive(lobbyManager.ReadyToStartGame); 
        }
    }
}