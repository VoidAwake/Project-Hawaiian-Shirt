using Hawaiian.Game;
using Hawaiian.Game.GameModes;
using UI.Core;
using UnityEngine;

namespace Hawaiian.UI.Game
{
    public class ModeUIManager : DialogueComponent<GameDialogue>
    {
        [SerializeField] private ModeManager modeManager;
        [SerializeField] private GameManager gameManager;

        protected override void Subscribe()
        {
            // TODO: This may be a race condition
            modeManager.initialised.AddListener(OnInitialised);
        }

        protected override void Unsubscribe()
        {
            modeManager.initialised.RemoveListener(OnInitialised);
        }

        private void OnInitialised(ModeController<ModeSceneReference> modeController)
        {
            var modeUIPrefab = gameManager.CurrentGameMode.modeUIPrefab;

            if (modeUIPrefab == null) return;
            
            var modeUIObject = Instantiate(modeUIPrefab, transform);

            var modeUI = modeUIObject.GetComponent<ModeUI<IModeController>>();

            modeUI.Initialise(modeController);
        }
    }
}