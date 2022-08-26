using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Hawaiian.Unit;
using Hawaiian.Utilities;
using PlasticGui.WorkspaceWindow;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Hawaiian.Inventory
{
    
    
    public enum TreasureState
    {
        Neutral,
        Vulnerable, // In this instance, vulnerable refers to the treasure in the process of being detonated, only in this state can it be defused
        Detonated
    }
    
    
    public class PlayerTreasure : MonoBehaviour
    {
        [SerializeField] private int _currentPoints;
        
        [SerializeField] private UnitPlayer _owner;
        [SerializeField] private TreasureState _currentState;
            
        //Properties
        public int CurrentPoints => _currentPoints;
        public TreasureState CurrentState => _currentState;

        public UnitPlayer Owner
        {
            get => _owner;
            set => _owner = value;
        }

        
        private void Start()
        {
            _currentState = TreasureState.Neutral;
        }

        public  void OnDetonatorStarted()
        {
            if (_currentState == TreasureState.Neutral)
                _currentState = TreasureState.Vulnerable;
        }
        
        public async void OnDetonatorCompleted()
        { 
            _currentState = TreasureState.Detonated;
            Debug.Log($"Player {_owner.PlayerNumber}'s treasure has been detonated!");
           await  TreasureUtil.BeginDetonatorTimer(1000);
           _currentState = TreasureState.Neutral;

        }

       
        public bool CanBeDetonated()
        {
            Debug.Log($"Can {_owner.name} be detonated: {_currentState == TreasureState.Neutral}");
            return _currentState == TreasureState.Neutral; 
        }

        
    }

}