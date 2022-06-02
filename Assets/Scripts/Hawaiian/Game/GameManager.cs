using Hawaiian.Utilities;
using UnityEngine;

namespace Hawaiian.Game
{
    [CreateAssetMenu(menuName = "Hawaiian/Managers/GameManager")]
    public class GameManager : ScriptableObject
    {
        public enum GamePhase { Stealth, HighAlert, GameOver }

        private GamePhase phase;

        public GamePhase Phase
        {
            get => phase;
            set
            {
                phase = value;
                phaseChanged.Raise();
                
                if (Phase == GamePhase.GameOver)
                    gameOver.Raise();
            }
        }

        public GameEvent phaseChanged;
        public GameEvent gameOver;
    }
}