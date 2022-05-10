using Hawaiian.Game;
using Hawaiian.UI.General;
using Hawaiian.Utilities;
using UnityEngine;

namespace Hawaiian.UI.Game
{
    public class StartGameButton : Button<GameDialogue>
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private ScriptableFloat gameTimeScale;
        [SerializeField] private GameObject toDisable;

        public override void OnClick()
        {
            base.OnClick();

            gameManager.Phase = GameManager.GamePhase.Stealth;
            
            gameTimeScale.Value = 1;
            
            toDisable.SetActive(false);
        }
    }
}