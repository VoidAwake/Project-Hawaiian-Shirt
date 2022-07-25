using Hawaiian.Game;
using Hawaiian.Game.GameModes;
using Hawaiian.UI.General;
using TMPro;
using UnityEngine;

namespace Hawaiian.UI.CharacterSelect
{
    public class ModeSelectButton : MenuButton<CharacterSelectDialogue>
    {
        [SerializeField] private TMP_Text text;

        public GameModeSO GameModeSO { get; set; }

        public void Initialise(GameModeSO gameModeSO)
        {
            GameModeSO = gameModeSO;
            
            text.text = gameModeSO.displayName;
        }
    }
}