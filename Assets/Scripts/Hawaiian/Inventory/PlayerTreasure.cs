    using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
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
        Vulnerable,
        Defusing,
        Detonated
    }

    public class PlayerTreasure : MonoBehaviour
    {
        public delegate void PointsChanged();

        public PointsChanged OnPointsChanged;

        public delegate void DefuseInitiated();

        public DefuseInitiated OnDefuseInitiated;

        public delegate void DefuseInterrupted();

        public DefuseInterrupted OnDefusedInterrupted;

        public delegate void DefuseCompleted();

        public DefuseCompleted OnDefuseCompleted;


        [SerializeField] private int _currentPoints;
        [SerializeField] private UnitPlayer _owner;
        [SerializeField] private TreasureState _currentState;

        [SerializeField] private float _defuserTimer;
        [SerializeField] private float _currentDefuseTimer;

        public float DefuseTimer => _defuserTimer;


        //Properties
        public int CurrentPoints
        {
            get => _currentPoints;
            set
            {
                _currentPoints = value;
                OnPointsChanged.Invoke();
            }
        }

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

        private void Update()
        {
            if (_currentState == TreasureState.Defusing)
                if (_currentDefuseTimer < _defuserTimer)
                    _currentDefuseTimer += Time.deltaTime;
                else
                    OnDefuseAchieved();
        }

        public void OnDetonatorStarted()
        {
            if (_currentState == TreasureState.Neutral)
                _currentState = TreasureState.Vulnerable;
        }

        public async void OnDetonatorCompleted()
        {
            OnDefuseInterrupted();
            _currentState = TreasureState.Detonated;
            Debug.Log($"Player {_owner.PlayerNumber}'s treasure has been detonated!");
            await TreasureUtil.BeginDetonatorTimer(5000);
            _currentState = TreasureState.Neutral;
        }

        public void OnDefuseStarted()
        {
            if (_currentState != TreasureState.Vulnerable)
                return;

            OnDefuseInitiated.Invoke();
            _currentState = TreasureState.Defusing;
        }

        public void OnDefuseInterrupted()
        {
            if (_currentState != TreasureState.Defusing )
                return;

            OnDefusedInterrupted.Invoke();
            _currentDefuseTimer = 0;
            _currentState = TreasureState.Vulnerable;
        }

        public void OnDefuseAchieved()
        {
            OnDefuseCompleted.Invoke();
            _currentState = TreasureState.Neutral;
            _currentDefuseTimer = 0;
        }

        // public UniTask BeginDepositing()
        // {
        //     
        // }


        public bool CanBeDetonated()
        {
            Debug.Log($"Can {_owner.name} be detonated: {_currentState == TreasureState.Neutral}");
            return _currentState == TreasureState.Neutral;
        }
        
        private bool CanDeposit(IUnit collidedUnit)
        {
            UnitPlayer unit = collidedUnit.GetUnit();

            if (unit != Owner)
                return false;

            InventoryController playerInventory = unit.GetComponentInChildren<InventoryController>();

            if (playerInventory == null)
                return false;

            return playerInventory.inv.GetAllTreasures().ToList().Count <= 0;
        }
    }
}