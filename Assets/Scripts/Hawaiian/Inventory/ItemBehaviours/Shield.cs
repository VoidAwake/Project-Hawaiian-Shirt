using System.Collections;
using Hawaiian.Unit;
using Hawaiian.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace Hawaiian.Inventory.ItemBehaviours
{
    public class Shield : ItemBehaviour
    {
        [SerializeField] [Tooltip("0 is down, 1 is up")]
        private Sprite[] _shieldState;

        public float ParryWindow { get; set; }
        public float TimeTillParry { get; set; }
        public int[] ParryPercentage { get; set; }

        private GameEvent parryOccured;
        private IUnitGameEventListener _listener;
        private ItemInteractor _itemInteractor;
        private UnitPlayer _user;
        private Transform _cursorPos;
        private Vector3 _lastPosition;
        private bool _canParry;

        private GameObject _shieldColliderPrefab;
    
        private GameObject _shieldColliderInstance;
        private SpriteRenderer _shieldSprite;
        private float _duration;
        private bool shieldLifted;


        public void Initialise(float parryWindow, int[] parryPercentage, SpriteRenderer spriteReference,
            Sprite[] shieldStates, IUnitGameEvent parryoccured, GameObject shieldCollider, float timeTillParry,
            UnitPlayer user, Transform cursorPos)
        {
            _cursorPos = cursorPos;
            _user = user;
            TimeTillParry = timeTillParry;
            _shieldColliderPrefab = shieldCollider;
            IUnitGameEventListener _eventListener = gameObject.AddComponent<IUnitGameEventListener>();
            _eventListener.UpdateEvent(parryoccured, new UnityEvent<IUnit>());
            _eventListener.Response.AddListener(ProcessParry);
            _shieldSprite = spriteReference;
            _shieldState = shieldStates;
            ParryWindow = parryWindow;
            ParryPercentage = parryPercentage;
            _duration = 0;
            _itemInteractor = GetComponent<ItemInteractor>();
            _canParry = true;
        }

        public void OnDisable()
        {
            if (_listener != null)
                _listener.Response.RemoveListener(ProcessParry);
        }

        public bool CanParry() => _canParry;

        public bool IsHit = false;

        //Refactor (maybe using generics?) since code is repeated twice
        public void ProcessParry(IUnit victim)
        {// Ensures that if the player is in the middle of a trip, if somehow a parry occured, it does not go through
            if (GetComponentInParent<UnitPlayer>().playerState == Unit.Unit.PlayerState.Tripped || IsHit)
            {
                IsHit = false;
                return;
            } 
        
        
            if (_shieldColliderInstance == null)
            {
                StartCoroutine(RemoveOverrideDamage());
                return;
            }

            _itemInteractor.PlayerReference.OverrideDamage = true;
        
            //Determine if the parry hit the desired window
            if ((_duration / ParryWindow) * 100 <= ParryPercentage[1] &&
                (_duration / ParryWindow) * 100 >= ParryPercentage[0])
            {
                //Perfect Parry
                Debug.Log("PERFECT PARRY");


                if (_shieldColliderInstance.GetComponent<ShieldCollider>() != null)
                {
                    ShieldCollider collider = _shieldColliderInstance.GetComponent<ShieldCollider>();
                
                    if (collider.ProjectileInstance != null)
                    {
                        collider.ProjectileInstance.UpdateTargetToFinalDestination(
                            victim.GetPosition(), 2f);
                    } else if (collider.ThrowableInstance != null)
                    {
                        collider.ThrowableInstance.UpdateTargetToFinalDestination(
                            victim.GetPosition() - transform.position);
                    }
                    else 
                    {
                        victim.TripUnit(victim.GetPosition() - transform.position, 8); //TODO Change distance
                        var inventoryController = victim.GetUnit().GetComponentInChildren<InventoryController>();

                        if (inventoryController != null)
                            inventoryController.DropRandom(-(victim.GetPosition() - transform.position));
                    }
                }
            }
            else
            {
                Debug.Log("PATRIAL PARRY");
            
            

                //Made into a switch or generic
                if (_shieldColliderInstance.GetComponent<ShieldCollider>() != null)
                {
                    ShieldCollider collider = _shieldColliderInstance.GetComponent<ShieldCollider>();
                    if (collider.ProjectileInstance != null)
                    {
                        collider.ProjectileInstance.UpdateTargetToFinalDestination(
                            victim.GetPosition(), 2f);
                    }else if (collider.ThrowableInstance != null)
                    {
                        collider.ThrowableInstance.UpdateTargetToFinalDestination(
                            victim.GetPosition() - transform.position);
                    }
                    else
                    {
                        victim.TripUnit(victim.GetPosition() - transform.position, 2); //TODO Change distance
                        var inventoryController = victim.GetUnit().GetComponentInChildren<InventoryController>();

                        if (inventoryController != null)
                            inventoryController.DropRandom(-(victim.GetPosition() - transform.position));
                    }
                }
            }

            StartCoroutine(RemoveOverrideDamage());
        }


        //Makes sure that if a parry is successful synchronous to the player being hit, that the hit will not occur
        IEnumerator RemoveOverrideDamage()
        {
            yield return new WaitForSeconds(Time.deltaTime);
            _itemInteractor.PlayerReference.OverrideDamage = false;
        }


        // If the unit was not hit during the parry, ensures that the override tag is removed


        public void LiftShield()
        {
            Debug.LogWarning("Currently Parrying");
            _lastPosition = _shieldSprite.gameObject.transform.localPosition;
            _shieldSprite.gameObject.transform.position = _cursorPos.position;
            shieldLifted = true;
            _canParry = false;
            _shieldColliderInstance = Instantiate(_shieldColliderPrefab, _cursorPos.transform.position, Quaternion.identity);
            _shieldSprite.sprite = _shieldState[1];
        }

        public void UnliftShield()
        {
            if (_shieldColliderInstance != null)
                Destroy(_shieldColliderInstance);

            StartCoroutine(ShieldCooldown(TimeTillParry));
            shieldLifted = false;
            _shieldSprite.sprite = _shieldState[0];
            _duration = 0;
            _shieldSprite.gameObject.transform.localPosition = _lastPosition;
        }

        // TODO: No longer used?
        public void RemoveShieldComponent()
        {
            if (_shieldColliderInstance != null)
                Destroy(_shieldColliderInstance);

            Destroy(this);
        }

        IEnumerator ShieldCooldown(float timer)
        {
            _canParry = false;
            yield return new WaitForSeconds(timer);
            _canParry = true;
        }


        private void Update()
        {
            if (shieldLifted)
            {
                _duration += Time.deltaTime;

                if (_duration > ParryWindow)
                {
                    UnliftShield();
                }
            }
        }
    }
}