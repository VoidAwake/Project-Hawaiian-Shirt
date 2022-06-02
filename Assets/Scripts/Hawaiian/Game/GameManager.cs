using System.Collections.Generic;
using Hawaiian.Utilities;
using UnityEngine;

namespace Hawaiian.Game
{
    [CreateAssetMenu(menuName = "Hawaiian/Managers/GameManager")]
    public class GameManager : ScriptableObject
    {
        [SerializeField] private SceneChanger sceneChanger;
        [SerializeField] private SceneReference resultsScene;
        [SerializeField] private List<SceneReference> levels;

        public enum GamePhase { Stealth, GameOver }

        private GamePhase phase;

        public GamePhase Phase
        {
            get => phase;
            set
            {
                phase = value;
                phaseChanged.Raise();
                
                if (Phase == GamePhase.GameOver)
                    GameOver();
            }
        }

        private void GameOver()
        {
            gameOver.Raise();
            
            sceneChanger.ChangeScene(resultsScene);
        }

        public GameEvent phaseChanged;
        public GameEvent gameOver;

        public void LoadRandomLevel()
        {
            if (levels.Count == 0) return;
            
            sceneChanger.ChangeScene(levels[Random.Range(0, levels.Count)]);
        }
    }
}