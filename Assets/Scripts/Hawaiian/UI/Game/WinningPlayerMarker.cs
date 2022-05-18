using Hawaiian.UI.CharacterSelect;
using UI.Core;
using UnityEngine;

namespace Hawaiian.UI.Game
{
    // TODO: Move out of UI
    // TODO: Or better still, set up a game space canvas for control prompts and this
    public class WinningPlayerMarker : MonoBehaviour
    {
        [SerializeField] private SpawnPlayers playerManager;
        [SerializeField] private Transform marker;
        [SerializeField] private Vector3 offset;
        
        private void Update()
        {
            if (playerManager.WinningPlayer == null) return;
            
            marker.position = playerManager.WinningPlayer.position + offset;
        }
    }
}