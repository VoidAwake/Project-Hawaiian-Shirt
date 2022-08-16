using UnityEngine;
using UnityEngine.UI;

namespace Hawaiian.UI.CharacterSelect
{
    public class CharacterPortraitCursor : MonoBehaviour
    {
        [SerializeField] private Image cursor;
        [SerializeField] private Sprite[] characterSelectSprites;
        
        private LobbyPlayerController lobbyPlayerController;

        private void Update()
        {
            if (lobbyPlayerController == null) return;
            
            if (lobbyPlayerController.Status == LobbyPlayerController.PlayerStatus.LoadedIn)
            {
                // Animate cursor
                cursor.sprite = characterSelectSprites
                [
                    Mathf.FloorToInt((Time.time % 0.39999f) * 10.0f) - (Mathf.FloorToInt((Time.time % 0.39999f) * 10.0f) > 2 ? 2 : 0)
                ];
            }
            else
            {
                // Set to still cursor
                cursor.sprite = characterSelectSprites[1];
            }
        }

        public void Initialise(LobbyPlayerController lobbyPlayerController)
        {
            this.lobbyPlayerController = lobbyPlayerController;
            
            lobbyPlayerController.statusChanged.AddListener(OnStatusChanged);
            lobbyPlayerController.characterUpdated.AddListener(OnCharacterUpdated);
            
            OnStatusChanged();
            OnCharacterUpdated();
        }

        private void OnStatusChanged()
        {
            cursor.enabled = lobbyPlayerController.Status != LobbyPlayerController.PlayerStatus.NotLoadedIn;
        }

        private void OnCharacterUpdated()
        {
            GetComponent<RectTransform>().anchoredPosition = lobbyPlayerController.Portrait.GetComponent<RectTransform>().anchoredPosition;
        }
    }
}