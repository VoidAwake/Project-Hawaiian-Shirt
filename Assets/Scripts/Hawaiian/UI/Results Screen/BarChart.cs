using System;
using System.Collections.Generic;
using System.Linq;
using Hawaiian.UI.CharacterSelect;
using Hawaiian.Unit;
using UI.Core;
using UnityEngine;
using UnityEngine.Events;

namespace Hawaiian.UI.Results_Screen
{
    public class BarChart : DialogueComponent<ResultsScreenDialogue>
    {
        [SerializeField] private GameObject playerBarPrefab;

        public UnityEvent animationCompleted = new UnityEvent();

        private readonly List<PlayerBar> bars = new List<PlayerBar>();
        private int currentBarIndex;
        
        protected override void Subscribe() { }

        protected override void Unsubscribe() { }

        public void AnimateBars()
        {
            if (bars.Count == 0) return;

            currentBarIndex = -1;

            StartNextAnimation();
        }

        private void OnAnimationCompleted()
        {
            StartNextAnimation();
        }

        private void StartNextAnimation()
        {
            if (currentBarIndex >= 0 && currentBarIndex < bars.Count)
            {
                var prevBar = bars[currentBarIndex];

                prevBar.animationCompleted.RemoveListener(OnAnimationCompleted);
            }

            currentBarIndex++;
                
            if (currentBarIndex >= 0 && currentBarIndex < bars.Count)
            {
                var bar = bars[currentBarIndex];

                bar.animationCompleted.AddListener(OnAnimationCompleted);

                bar.StartAnimation();
            }
            else
            {
                animationCompleted.Invoke();
            }
        }

        private void CreatePlayerBar(LobbyGameManager.PlayerConfig player)
        {
            if (!player.isPlayer) return;
            
            var playerBarObject = Instantiate(playerBarPrefab, transform);

            var playerBar = playerBarObject.GetComponent<PlayerBar>();

            if (playerBar == null) throw new Exception($"Player Bar prefab does not have a {nameof(PlayerBar)} component.");

            playerBar.Initialise(player);

            bars.Add(playerBar);
        }

        public void Initialise(LobbyGameManager playersData)
        {
            var sortedPlayers = playersData.playerConfigs.ToList();
            
            // TODO: Won't work for some float scores
            sortedPlayers.Sort((a, b) => Mathf.CeilToInt(a.score - b.score));
            
            foreach (var player in sortedPlayers)
            {
                CreatePlayerBar(player);
            }
        }
    }
}