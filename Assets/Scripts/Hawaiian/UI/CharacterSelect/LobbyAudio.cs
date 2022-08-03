using UnityEngine;
using UnityEngine.Events;

namespace Hawaiian.UI.CharacterSelect
{
    public class LobbyAudio : MonoBehaviour
    {
        [SerializeField] private SuperLobbyManager superLobbyManager;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip characterUpdatedAudio;
        [SerializeField] private AudioClip playerReadyAudio;
        [SerializeField] private AudioClip playerJoinedAudio;

        private void OnEnable()
        {
            superLobbyManager.playerJoined.AddListener(OnPlayerJoined);

            foreach (var lobbyPlayerController in superLobbyManager.lobbyPlayerControllers)
            {
                OnPlayerJoined(lobbyPlayerController);
            }
        }

        private void OnDisable()
        {
            foreach (var lobbyPlayerController in superLobbyManager.lobbyPlayerControllers)
            {
                lobbyPlayerController.characterUpdated.RemoveListener(OnCharacterUpdated);
                lobbyPlayerController.statusChanged.RemoveListener(OnStatusChanged);
            }
        }

        private void OnPlayerJoined(LobbyPlayerController lobbyPlayerController)
        {
             lobbyPlayerController.characterUpdated.AddListener(OnCharacterUpdated);
             lobbyPlayerController.statusChanged.AddListener(OnStatusChanged);
             audioSource.PlayOneShot(playerJoinedAudio);
        }

        private void OnStatusChanged(LobbyPlayerController lobbyPlayerController)
        {
            if (lobbyPlayerController.Status == LobbyPlayerController.PlayerStatus.Ready)
                audioSource.PlayOneShot(playerReadyAudio);
        }

        private void OnCharacterUpdated()
        {
            audioSource.PlayOneShot(characterUpdatedAudio);
        }
    }
}