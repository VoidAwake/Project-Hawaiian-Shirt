using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Hawaiian.UI.Utilities
{
    [RequireComponent(typeof(Slider))]
    public class ExtendedSlider : MonoBehaviour
    {
        [SerializeField] internal bool _hideOnStartup;
        
        private Slider _slider;

        private void OnEnable()
        {
            _slider = GetComponent<Slider>();
            
            if (_hideOnStartup)
                Hide();
        }

        internal async UniTask BeginSliderAnim(float startTime, float endTime, CancellationToken token = default)
        {
            float currentTime = startTime;
            _slider.value = startTime;

            while (currentTime < endTime)
            {
                currentTime += Time.deltaTime;
                _slider.value = (currentTime / endTime) ;
                await UniTask.Yield(token);
            }

            _slider.value = endTime;
        }


        internal async UniTask BeginSliderAnim(float duration, CancellationToken token) => await BeginSliderAnim(0f, duration, token);


        internal bool StopSliderAnim(CancellationTokenSource token, bool resetTimer = false)
        {
            if (token is null)
                return false;

            if (resetTimer)
                _slider.value = 0;

            token.Cancel();
            return true;
        }

        internal void Hide() => _slider.gameObject.SetActive(false);

        internal void Show() => _slider.gameObject.SetActive(true);

        internal bool IsHidden() => _slider.isActiveAndEnabled;

        internal float GetCurrentValue() => _slider.value;

        internal float SetCurrentValue(float newValue) => _slider.value = newValue;

    }
}