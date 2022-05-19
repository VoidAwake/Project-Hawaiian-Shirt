﻿using Hawaiian.Utilities;
using UI.Core;

namespace Hawaiian.UI.General
{
    public class CloseDialogueButton : Button<Dialogue>
    {
        public GameEvent DialogueClosed;
        
        public override void OnClick()
        {
            base.OnClick();
            DialogueClosed.Raise();
            manager.Pop();
        }
    }
}