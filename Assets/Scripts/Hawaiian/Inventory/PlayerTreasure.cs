using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Hawaiian.Unit;
using UnityEngine;
using Random = System.Random;
using Vector2 = System.Numerics.Vector2;

namespace Hawaiian.Inventory
{
    public enum TreasureState
    {
        Neutral,
        Vulnerable,
        Defusing,
        Detonated,
        Depositing
    }

    public class PlayerTreasure : MonoBehaviour
    {
        #region Events

        public delegate int PointsChanged(int points);

        public PointsChanged OnPointsChanged;

        public delegate void DefuseInitiated();

        public DefuseInitiated OnDefuseInitiated;

        public delegate void DefuseInterrupted();

        public DefuseInterrupted OnDefusedInterrupted;

        public delegate void DefuseCompleted();

        public DefuseCompleted OnDefuseCompleted;

        public delegate void OnItemDeposited();

        public event DefuseCompleted ItemDeposited;


        public delegate void OnItemDepositedStopped();

        public event DefuseCompleted ItemDepositedStopped;

        #endregion

        [Header("Components")] [SerializeField]
        private TreasureHitbox _hitbox;

        [SerializeField] private TreasureAnimationController _animController;

        [Header("Owner")] [SerializeField] private UnitPlayer _owner;

        [Header("Treasure Stats")] [SerializeField]
        private TreasureState _currentState;

        [Header("Item References")]
        [SerializeField] private Item _itemReference;
        [SerializeField] private GameObject _droppedItemReference;


        [SerializeField] private float _defuserTimer;
        [SerializeField] private float _currentDefuseTimer;
        [SerializeField] private float _currentPoints;

        private IUnit _currentCollidedUnit;

        public TreasureHitbox Hitbox => _hitbox;

        public float DefuseTimer => _defuserTimer;

        public float CurrentPoints
        {
            get => _currentPoints;
            set
            {
                _currentPoints = value;
                OnPointsChanged.Invoke((int) _currentPoints); // lol cant be bothered to change it properly
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


            _owner.tripped?.AddListener(() =>
            {
                switch (_currentState)
                {
                    case TreasureState.Depositing:
                        _hitbox.DepositToken?.Cancel();
                        break;
                    case TreasureState.Defusing:
                        OnDefusedInterrupted();
                        break;
                }
            });
        }

        private void OnEnable()
        {
            if (_hitbox == null)
            {
                _hitbox = GetComponentInChildren<TreasureHitbox>();
                if (_hitbox == null)
                {
                    Debug.LogWarning(
                        "The component Treasure Hitbox was not found the player treasure component may not work as intended");
                }
            }

            if (_animController == null)
            {
                _animController = GetComponentInChildren<TreasureAnimationController>();

                if (_animController == null)
                {
                    Debug.LogWarning(
                        "The component Animation Controller was not found! the player treasure component may not work as intended");
                }
            }

            _hitbox.DepositStarted +=
                UniTask.UnityAction(async () => await DepositItems(_hitbox.DepositToken.Token));

            _hitbox.CollidedUnit += unit =>
            {
                _currentCollidedUnit = unit;

                // if (_currentCollidedUnit == null)
                // {
                //     _currentCollidedUnit = unit;
                //     return;
                // }
                //
                // //If the current collided unit is already the owner, if a new unit tries to deposit on it then it will be ignored
                // if (_currentCollidedUnit.GetUnit() == _owner && unit != _currentCollidedUnit)
                //     return;
                //
                // _currentCollidedUnit = unit;
            };
        }


        private void OnDisable()
        {
            if (_hitbox == null)
                return;

            _hitbox.DepositStarted -= UniTask.UnityAction(async () => await DepositItems(_hitbox.DepositToken.Token));

            _owner.tripped?.RemoveListener(() =>
            {
                switch (_currentState)
                {
                    case TreasureState.Depositing:
                        _hitbox.DepositToken?.Cancel();
                        break;
                    case TreasureState.Defusing:
                        OnDefusedInterrupted();
                        break;
                }
            });
        }

        private void Update()
        {
            if (_currentState != TreasureState.Defusing)
                return;

            if (_owner.playerState == Unit.Unit.PlayerState.Tripped)
            {
                OnDefusedInterrupted();
                return;
            }

            if (_currentDefuseTimer < _defuserTimer)
                _currentDefuseTimer += Time.deltaTime;
            else
                OnDefuseAchieved();
        }

        private void GenerateAndRemoveDetonatedItems()
        {
            ICollection<int> treasurePoints = TreasureUtil.GetDetonatedItemsData((int) CurrentPoints);

            for (int i = 0; i < treasurePoints.Count; i++)
            {
                Item treasureItem = ScriptableObject.CreateInstance<Item>();
                treasureItem.ItemName = _itemReference.ItemName;
                treasureItem.ItemSprite = _itemReference.ItemSprite;
                treasureItem.DroppedItemSprite = _itemReference.DroppedItemSprite;
                treasureItem.Type = ItemType.Objective;
                treasureItem.Points = treasurePoints.ToList()[i];
                treasureItem.DroppedItemBase = _droppedItemReference;

             
                

               GameObject droppedItem = Instantiate(_droppedItemReference, transform.position,Quaternion.identity);
               droppedItem.GetComponent<DroppedItem>().Item = treasureItem;

               // I'm so sorry too, im not the better man that i thought i was (its a simple fix too)
               for (int j = 0; j < droppedItem.transform.childCount; j++)
               {
                   if (droppedItem.transform.GetChild(j).name == "Item Sprite")
                   {
                       droppedItem.transform.GetChild(j).GetComponent<SpriteRenderer>().sprite =
                           treasureItem.ItemSprite;
                   }
               }

               // UnityEngine.Vector2 randomDirection =
               //     new UnityEngine.Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));

               UnityEngine.Vector2 randomDirection = UnityEngine.Random.insideUnitCircle;
               
               droppedItem.GetComponent<ItemUnit>().OnThrow(randomDirection, 2f);
               CurrentPoints -= treasureItem.Points;
            }
        }

        public void OnDetonatorStarted()
        {
            if (_currentState == TreasureState.Neutral || _currentState == TreasureState.Depositing)
                _currentState = TreasureState.Vulnerable;

            if (_hitbox != null)
                _hitbox.DepositToken?.Cancel(); // cancels depositing if detonation has begun
        }

        public async void OnDetonatorCompleted()
        {
            CancellationTokenSource _source = new CancellationTokenSource(); // in case in the future it needs to stop for some reason
            OnDefuseInterrupted();
            _currentState = TreasureState.Detonated;
            GenerateAndRemoveDetonatedItems();
            Debug.Log($"Player {_owner.PlayerNumber}'s treasure has been detonated!");
            await _animController.PlayChestDetonationAnimation(_source.Token);
            await TreasureUtil.BeginDetonatorTimer(5000);
            await _animController.PlayChestClosingAnim(_source.Token);
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
            if (_currentState != TreasureState.Defusing)
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


        public bool CanBeDetonated()
        {
            Debug.Log(
                $"Can {_owner.name} be detonated: {_currentState == TreasureState.Neutral || _currentState == TreasureState.Depositing}");
            return _currentState == TreasureState.Neutral || _currentState == TreasureState.Depositing;
        }

        public async UniTask DepositItems(CancellationToken token)
        {
            //inital check to ensure the owner is depositing and the animations can play as normal
            if (_currentCollidedUnit.GetUnit() != _owner || _currentCollidedUnit == null || (_currentState != TreasureState.Depositing && _currentState != TreasureState.Neutral))
                return;

            InventoryController controller = _owner.GetComponentInChildren<InventoryController>();
            int treasureAmount = controller.inv.GetAllTreasures().ToList().Count;

            if (treasureAmount == 0) // could do a prompt to let the player know they need treasure to deposit
                return;

            try
            {
                await _animController.PlayChestOpeningAnim(token);
                _currentState = TreasureState.Depositing;

                for (int i = 0; i < treasureAmount; i++)
                {
                    if (CanDeposit(_owner))
                    {
                        Debug.Log($"Depositing item: {controller.inv.GetMostLeftItem().Value.ItemName}");
                        DepositItem(controller.inv.GetMostLeftItem().Value, controller,
                            controller.inv.GetMostLeftItem().Key);
                        await _animController
                            .PlayChestDepositAnim(
                                token); // minimum time the deposit has to wait before it can deposit again is based on the animation
                    }

                    await UniTask.Delay(250, false, PlayerLoopTiming.Update, token);
                }

                ItemDepositedStopped?.Invoke();
                _currentState = TreasureState.Neutral;
                await _animController.PlayChestClosingAnim(token);
                
            }
            catch (OperationCanceledException e)
            {
                ItemDepositedStopped?.Invoke();
                _currentState = TreasureState.Neutral;
                await _animController.PlayChestClosingAnim(token);
                Debug.Log($"Depositing was cancelled {e}");
            }
        }

        public bool CanDeposit(IUnit collidedUnit)
        {
            if (collidedUnit == null)
                return false;

            UnitPlayer unit = collidedUnit.GetUnit();


            if (unit != Owner)
                return false;

            InventoryController playerInventory = unit.GetComponentInChildren<InventoryController>();

            if (playerInventory == null)
                return false;

            if (playerInventory.inv.GetAllTreasures().ToList().Count <= 0)
                return false;

            return true;
        }

        public void DepositItem(Item item, InventoryController controller = null, int itemPos = 0)
        {
            CurrentPoints += item.Points;
            ItemDeposited?.Invoke();

            if (controller == null)
                return;

            controller.inv.RemoveItemAt(itemPos);
        }
    }
}