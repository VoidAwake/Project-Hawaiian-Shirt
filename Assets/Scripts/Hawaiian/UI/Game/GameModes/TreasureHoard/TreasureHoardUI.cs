using Hawaiian.Game;
using Hawaiian.Game.GameModes.TreasureHoard;
using UnityEngine;

namespace Hawaiian.UI.Game
{
    public class TreasureHoardUI : ModeUI<TreasureHoardModeManager>
    {
        [SerializeField] private GameObject TreasureHoardUIPrefab;
        [SerializeField] private Transform TreasureHoardUIParent;

        protected override void Subscribe()
        {
            base.Subscribe();

            modeController.playerJoined.AddListener(GenerateTreasurePointUI);
        }

        protected override void Unsubscribe()
        {
            base.Unsubscribe();
            
            modeController.playerJoined.RemoveListener(GenerateTreasurePointUI);
        }

        private void GenerateTreasurePointUI(PlayerConfig playerConfig)
        {
            GameObject treasurePointIndicatorObject = Instantiate(TreasureHoardUIPrefab, TreasureHoardUIParent);

            var treasurePointIndicator = treasurePointIndicatorObject.GetComponent<TreasurePointIndicator>();
            
         //   treasurePointIndicator.Initialise(modeController.PlayerTreasures[playerConfig]);
        }
    }
}