using System;
using System.Collections.Generic;
using Hawaiian.Game.GameModes;
using UI.Core;
using UnityEngine;

namespace Hawaiian.UI.Game
{
    public class ModeUIManager : DialogueComponent<GameDialogue>
    {
        // TODO: See if we can do this better
        [SerializeField] private List<UIBinding> uiBindings;
        
        [Serializable]
        private struct UIBinding {
            public string modeManagerTypeName;
            public GameObject modeUIPrefab;
        }

        protected override void Subscribe()
        {
            // TODO: This may be a race condition
            ModeManager.Initialised.AddListener(OnInitialised);
            
            OnInitialised(ModeManager.CurrentModeManager);
        }

        protected override void Unsubscribe()
        {
            ModeManager.Initialised.RemoveListener(OnInitialised);
        }

        private void OnInitialised(ModeManager modeManager)
        {
            var modeManagerType = ModeManager.CurrentModeManager.GetType();

            var uiBinding = uiBindings.Find(b => b.modeManagerTypeName == modeManagerType.ToString());

            var modeUIPrefab = uiBinding.modeUIPrefab;
            
            if (modeUIPrefab == null) return;
            
            Instantiate(modeUIPrefab, transform);
        }
    }
}