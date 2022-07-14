using System.Linq;
using Hawaiian.Game;
using UnityEngine;

namespace Hawaiian.UI.Game
{
    public class TreasureHoardUI : ModeUI
    {
        [SerializeField] private GameObject TreasureHoardUIPrefab;
        [SerializeField] private Transform TreasureHoardUIParent;
        
        private TreasureHoardModeController modeController;

        public override void Initialise(IModeController modeController)
        {
            base.Initialise(modeController);
            
            this.modeController = (TreasureHoardModeController) modeController;
        }

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