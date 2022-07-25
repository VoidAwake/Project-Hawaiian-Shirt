using Hawaiian.Game.GameModes;
using Hawaiian.Utilities;
using UnityEngine;

namespace Hawaiian.Game
{
    [CreateAssetMenu(menuName = "Hawaiian/Managers/GameManager")]
    public class GameManager : ScriptableObject
    {
        [SerializeField] private SceneChanger sceneChanger;
        [SerializeField] private SceneReference resultsScene;

        public enum GamePhase { Stealth, GameOver }

        public GameModeSO CurrentGameMode { get; set; }

        public ModeSceneReference ModeSceneReference { get; private set; }

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
            var levelSet = CurrentGameMode.sceneReferences;
            
            if (levelSet.Count == 0) return;

            ModeSceneReference = levelSet[Random.Range(0, levelSet.Count)];
            
            sceneChanger.ChangeScene(ModeSceneReference.sceneReference);
        }
    }
}