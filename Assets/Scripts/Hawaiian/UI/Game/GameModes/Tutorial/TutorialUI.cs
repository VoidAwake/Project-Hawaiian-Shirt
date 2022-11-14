using Hawaiian.Game.GameModes.Tutorial;
using TMPro;
using UnityEngine;

namespace Hawaiian.UI.Game.GameModes.Tutorial
{
    public class TutorialUI : ModeUI<TutorialModeManager>
    {
        [SerializeField] private TMP_Text text;
        
        protected override void Subscribe()
        {
            base.Subscribe();

            modeController.textChanged.AddListener(OnTextChanged);
            
            OnTextChanged();
        }

        protected override void Unsubscribe()
        {
            base.Unsubscribe();
            
            modeController.textChanged.RemoveListener(OnTextChanged);
        }

        private void OnTextChanged()
        {
            text.text = modeController.Text;
        }
    }
}