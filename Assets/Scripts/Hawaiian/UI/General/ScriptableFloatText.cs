using System;
using Hawaiian.Utilities;
using TMPro;
using UI.Core;
using UnityEngine;

namespace Hawaiian.UI.General
{
    public class ScriptableFloatText : DialogueComponent<Dialogue>
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private ScriptableFloat scriptableFloat;
        [SerializeField] private string format;

        private void OnValueChanged()
        {
            text.text = String.Format(format, scriptableFloat.Value);
        }

        protected override void Subscribe()
        {
            scriptableFloat.valueChanged.AddListener(OnValueChanged);

            try
            {
                OnValueChanged();
            }
            catch (FormatException)
            {
                enabled = false;
                
                throw;
            }
        }

        protected override void Unsubscribe()
        {
            scriptableFloat.valueChanged.RemoveListener(OnValueChanged);
        }
    }
}