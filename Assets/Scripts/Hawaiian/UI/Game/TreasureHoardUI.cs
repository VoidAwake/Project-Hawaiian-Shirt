using Hawaiian.Game;
using UI.Core;
using UnityEngine;

namespace Hawaiian.UI.Game
{
    public class TreasureHoardUI : DialogueComponent<GameDialogue>
    {
        [SerializeField] private GameObject TreasureHoardUIPrefab;
        [SerializeField] private Transform TreasureHoardUIParent;
        [SerializeField] private PlayerManager playerManager;

        public void GenerateTreasurePointUI()
        {
            GameObject treasurePrefab = Instantiate(TreasureHoardUIPrefab, TreasureHoardUIParent);
            treasurePrefab.GetComponent<TreasurePointIndicator>().Initialise(playerManager.LastPlayerJoined);
        }

        protected override void Subscribe() { }

        protected override void Unsubscribe() { }
    }
}