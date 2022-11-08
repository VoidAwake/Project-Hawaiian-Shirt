using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Hawaiian.Inventory;
using TMPro;
using UI.Core;
using UnityEngine;
     namespace Hawaiian.UI.Item
{
    [RequireComponent(typeof(Canvas))]
    public class DetonatorUI : Dialogue
    {
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private Detonator _detonatorReference;
        
        [Tooltip("Calculated in milliseconds")]
        [SerializeField] private int _countdownDuration = 5000;

        private CancellationTokenSource cts;
        
        private  void OnEnable()
        {
            _detonatorReference = GetComponentInParent<Detonator>();
        }

        private void Update()
        {
            _timerText.text = ((int)_detonatorReference.CurrentDetonationTime).ToString();
        }

        protected override void OnClose() { }

        protected override void OnPromote() { }

        protected override void OnDemote() { }

      
    }
}
