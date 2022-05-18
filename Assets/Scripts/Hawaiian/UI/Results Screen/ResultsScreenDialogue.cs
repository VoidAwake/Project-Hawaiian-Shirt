﻿using System.Collections;
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
        
        private LobbyGameManager.PlayerConfig playersData;

        private void Start()
        {
            LobbyGameManager playerData = FindObjectOfType<LobbyGameManager>();
            
            barChart.Initialise(playerData);
            
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