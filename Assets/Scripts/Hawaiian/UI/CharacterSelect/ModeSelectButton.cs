using Hawaiian.Game.GameModes;
using Hawaiian.UI.General;
using TMPro;
using UnityEngine;

namespace Hawaiian.UI.CharacterSelect
{
    public class ModeSelectButton : MenuButton<CharacterSelectDialogue>
    {
        [SerializeField] private TMP_Text text;

        public ModeManager ModeManager { get; set; }

        public void Initialise(ModeManager modeManager)
        {
            ModeManager = modeManager;
            
            text.text = modeManager.DisplayName;
        }
    }
}