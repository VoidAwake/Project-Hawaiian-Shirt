using System.Collections;
using Hawaiian.Game;
using Hawaiian.UI.CharacterSelect;
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
        [SerializeField] private PlayerConfig[] defaultPlayerConfigs;
        [SerializeField] private PlayerConfigManager playerConfigManager;

        protected override void Start()
        {
            base.Start();
            
            var playerConfigs = playerConfigManager != null ? playerConfigManager.playerConfigs : defaultPlayerConfigs;

            barChart.Initialise(playerConfigs);
            
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