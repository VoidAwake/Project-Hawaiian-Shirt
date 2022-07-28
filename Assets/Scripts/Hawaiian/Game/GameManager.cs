using System;
using System.Threading.Tasks;
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
        [SerializeField] private GameEvent gameOver;
        
        public event Func<object, EventArgs, Task> GameOverAsync;

        public ModeManager CurrentGameMode { get; set; }

        public async void GameOver()
        {
            gameOver.Raise();

            // TODO: Not great to have to bypass the GameEvent system like this
            await GameOverAsync?.Invoke(this, EventArgs.Empty);
            
            await sceneChanger.ChangeScene(resultsScene);
        }
    }
}