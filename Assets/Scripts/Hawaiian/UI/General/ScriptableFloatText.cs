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
         ScriptableFloat minute, sec, msec;
        private void OnValueChanged()
        {
            text.text = string.Format(format, (int)scriptableFloat.Value / 60 % 60, ((int)scriptableFloat.Value % 60).ToString("00"), ((int)(Mathf.Floor(scriptableFloat.Value * 100) % 100)).ToString("00"));
           
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