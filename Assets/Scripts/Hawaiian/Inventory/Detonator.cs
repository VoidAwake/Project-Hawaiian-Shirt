using System;
using System.Collections;
using System.Globalization;
using System.Threading;
using Cysharp.Threading.Tasks;
using Hawaiian.PositionalEvents;
using Hawaiian.Unit;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hawaiian.Inventory
{
    public class Detonator : MonoBehaviour
    {
        [SerializeField] private GameObject _detonationCallerReference;
        [SerializeField] private int _countdownDuration;

        private CancellationTokenSource _cts;
        private PositionalEventCaller _caller;

        public float CurrentDetonationTime { get; set; }

        private void OnEnable()
        {
            _caller = GetComponent<PositionalEventCaller>();
            BeginCountdown();
        }

        private void Start()
        {
            _caller.OnRegisterTarget += _caller.Raise;
        }

        private async void BeginCountdown()
        {
            _cts = new CancellationTokenSource();

            try
            {
                await CountdownDetonation(_countdownDuration, _cts.Token);
            }
            catch (OperationCanceledException ex)
            {
                Debug.Log($"The detonation was cancelled: {ex} ");
            }
        }

        [ContextMenu("Cancel Detonation")]
        public void CancelDetonation() => _cts?.Cancel();


        private async UniTask CountdownDetonation(int duration, CancellationToken token = default)
        {
            duration /= 1000; //Converts to seconds

            float startTime = duration;
            CurrentDetonationTime = startTime;
            float endTime = 0;

            while (CurrentDetonationTime > endTime)
            {
                CurrentDetonationTime -= Time.deltaTime;
                await UniTask.Yield(token);
            }

            OnDetonation();
        }


        private void OnDetonation()
        {
            var _completedDetonator = Instantiate(_detonationCallerReference);
            PositionalEventCaller caller = _completedDetonator.GetComponent<PositionalEventCaller>();

            if (caller != null)
                caller.OnRegisterTarget += (listener =>
                {
                    if (!listener.gameObject.GetComponent<PlayerTreasure>())
                        return;
                    
                    caller.Raise();
                    Destroy(_completedDetonator.gameObject);
                    Destroy(gameObject);
                });
        }
    }
}