using System;
using Hawaiian.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hawaiian.Game
{
    [CreateAssetMenu(menuName = "Hawaiian/Managers/GameManager", fileName = "NewGameManager")]
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
            }
        }

        public GameEvent phaseChanged;
    }
}