using System.Linq;
using Hawaiian.Game.GameModes.TreasureHoard;
using UnityEngine;

namespace Hawaiian.UI.Game
{
    public class TreasureHoardUI : ModeUI<TreasureHoardModeController>
    {
        [SerializeField] private GameObject TreasureHoardUIPrefab;
        [SerializeField] private Transform TreasureHoardUIParent;

        public void GenerateTreasurePointUI()
        {
            GameObject treasurePointIndicatorObject = Instantiate(TreasureHoardUIPrefab, TreasureHoardUIParent);

            var treasurePointIndicator = treasurePointIndicatorObject.GetComponent<TreasurePointIndicator>();
            
            treasurePointIndicator.Initialise(modeController.PlayerTreasures.Last().Value);
        }

        protected override void Subscribe() { }

        protected override void Unsubscribe() { }
    }
}