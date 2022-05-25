using System.Collections.Generic;
using System.Linq;
using Hawaiian.UI.CharacterSelect;
using UnityEngine;

namespace Hawaiian.UI.Game
{
    // TODO: Move out of UI
    // TODO: Or better still, set up a game space canvas for control prompts and this
    public class WinningPlayerMarker : MonoBehaviour
    {
        [SerializeField] private SpawnPlayers playerManager;
        [SerializeField] private GameObject markerPrefab;
        [SerializeField] private Vector3 offset;

        private Dictionary<Transform, Transform> markers = new();

        private void OnEnable()
        {
            playerManager.winningPlayersChanged.AddListener(OnWinningPlayersChanged);
        }

        private void OnDisable()
        {
            playerManager.winningPlayersChanged.RemoveListener(OnWinningPlayersChanged);
        }

        private void Update()
        {
            foreach (var (source, dest) in markers)
            {
                dest.position = source.position + offset;
            }
        }

        private void OnWinningPlayersChanged()
        {
            var oldWinningPlayers = markers.Keys.ToList();
            var newWinningPlayers = playerManager.WinningPlayers;

            foreach (var newWinningPlayer in newWinningPlayers)
            {
                if (!oldWinningPlayers.Contains(newWinningPlayer))
                    AddMarker(newWinningPlayer);
            }

            foreach (var oldWinningPlayer in oldWinningPlayers)
            {
                if (!newWinningPlayers.Contains(oldWinningPlayer))
                    RemoveMarker(oldWinningPlayer);
            }
        }

        private void AddMarker(Transform player)
        {
            // TODO: Why don't we just put it on the player?
            // TODO: We would need to make sure it was properly disposed if the player was destroyed.
            // TODO: That would mean we could also remove the offset.
            var markerGameObject = Instantiate(markerPrefab, transform);
            
            markers.Add(player, markerGameObject.transform);
        }

        private void RemoveMarker(Transform player)
        {
            Destroy(markers[player].gameObject);

            markers.Remove(player);
        }
    }
}