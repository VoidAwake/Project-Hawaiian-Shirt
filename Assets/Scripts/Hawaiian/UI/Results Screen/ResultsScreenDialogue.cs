using System.Collections;
using Hawaiian.Unit;
using UI.Core;
using UnityEngine;

namespace Hawaiian.UI.Results_Screen
{
    public class ResultsScreenDialogue : Dialogue
    {
        [SerializeField] private BarChart barChart;
        [SerializeField] private Animator animator;
        [SerializeField] private float startDelayDuration;
        // TODO: Pull this from PlayerPrefs
        [SerializeField] private PlayersData playersData;

        private void Start()
        {
            barChart.Initialise(playersData);
            
            StartCoroutine(Animate());
        }

        protected override void OnClose() { }

        protected override void OnPromote() { }

        protected override void OnDemote() { }

        private IEnumerator Animate()
        {
            yield return new WaitForSeconds(startDelayDuration);
            
            barChart.animationCompleted.AddListener(() => animator.SetTrigger("ShowButtons"));
            
            barChart.AnimateBars();
        }
    }
}