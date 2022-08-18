using System.Collections;
using Hawaiian.Inventory.ItemBehaviours;
using Hawaiian.Unit;
using Hawaiian.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Hawaiian.Inventory.HeldItemBehaviours
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class ItemShield : HeldItemBehaviour
    {
        [SerializeField] private IUnitGameEvent _parryOccured;
        private SpriteRenderer heldItemSpriteRenderer;

        public override void Initialise(ItemHolder itemHolder)
        {
            base.Initialise(itemHolder);
            
            IUnitGameEventListener _eventListener = gameObject.AddComponent<IUnitGameEventListener>();
            _eventListener.UpdateEvent(_parryOccured, new UnityEvent<IUnit>());
            _eventListener.Response.AddListener(ProcessParry);
            _duration = 0;
            _canParry = true;
        }

        protected override void UseItemActionPerformed(InputAction.CallbackContext value)
        {
            if (CanParry())
                LiftShield();
        }
        
        // TODO: From Shield
        private Sprite[] _shieldState => new[] { Item.ShieldDown, Item.ShieldUp };

        public float ParryWindow => Item.ParryWindow;
        public float TimeTillParry => Item.TimeTillParry;
        public int[] ParryPercentage => new[] { Item.ParryPercentageUpperLimit, Item.ParryPercentageLowerLimit };

        private GameEvent parryOccured;
        private IUnitGameEventListener _listener;
        private Transform _cursorPos => Cursor.transform;
        private Vector3 _lastPosition;
        private bool _canParry;

        private float _duration;
        private bool shieldLifted;

        public void OnDisable()
        {
            if (_listener != null)
                _listener.Response.RemoveListener(ProcessParry);
        }

        public bool CanParry() => _canParry;

        public bool IsHit = false;

        // TODO: Refactor (maybe using generics?) since code is repeated twice
        public void ProcessParry(IUnit victim)
        {// Ensures that if the player is in the middle of a trip, if somehow a parry occured, it does not go through
            if (GetComponentInParent<UnitPlayer>().playerState == Unit.Unit.PlayerState.Tripped || IsHit)
            {
                IsHit = false;
                return;
            } 
        
            UnitPlayer.OverrideDamage = true;
            
             //TODO Change distance
            var victimTripDistance = IsPerfectParry() ? 8 : 2;
            
            Parry(victim, victimTripDistance);

            StartCoroutine(RemoveOverrideDamage());
        }

        private void Parry(IUnit victim, float victimTripDistance)
        {
            // TODO: Make into a switch or generic
            if (ProjectileInstance != null)
            {
                ProjectileInstance.UpdateTargetToFinalDestination(
                    victim.GetPosition(), 2f);
            }
            else if (ThrowableInstance != null)
            {
                ThrowableInstance.UpdateTargetToFinalDestination(
                    victim.GetPosition() - transform.position);
            }
            else // Melee Weapon
            {
                victim.TripUnit(victim.GetPosition() - transform.position, victimTripDistance);
                var inventoryController = victim.GetUnit().GetComponentInChildren<InventoryController>();

                if (inventoryController != null)
                    inventoryController.DropRandom(-(victim.GetPosition() - transform.position));
            }
        }

        private bool IsPerfectParry()
        {
            return (_duration / ParryWindow) * 100 <= ParryPercentage[1] &&
                   (_duration / ParryWindow) * 100 >= ParryPercentage[0];
        }


        //Makes sure that if a parry is successful synchronous to the player being hit, that the hit will not occur
        IEnumerator RemoveOverrideDamage()
        {
            yield return null;
            UnitPlayer.OverrideDamage = false;
        }


        // If the unit was not hit during the parry, ensures that the override tag is removed


        public void LiftShield()
        {
            Debug.LogWarning("Currently Parrying");
            _lastPosition = heldItemSpriteRenderer.gameObject.transform.localPosition;
            heldItemSpriteRenderer.gameObject.transform.position = _cursorPos.position;
            shieldLifted = true;
            _canParry = false;
            heldItemSpriteRenderer.sprite = _shieldState[1];
        }

        public void UnliftShield()
        {
            StartCoroutine(ShieldCooldown(TimeTillParry));
            shieldLifted = false;
            heldItemSpriteRenderer.sprite = _shieldState[0];
            _duration = 0;
            heldItemSpriteRenderer.gameObject.transform.localPosition = _lastPosition;
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
        
        // TODO: From ShieldCollider
        public Projectile ProjectileInstance;
        public Throwable ThrowableInstance;

        private void OnTriggerEnter2D(Collider2D col)
        {
            //TODO: Update itembehaviour to have a reference to their user
            var projectile = col.gameObject.GetComponent<Projectile>();
            if (projectile != null)
            {
                ProjectileInstance = projectile;
                _parryOccured.Raise(projectile.User);
            }
            else if (col.gameObject.GetComponent<DamageIndicator>() != null)
                _parryOccured.Raise(col.gameObject.GetComponent<DamageIndicator>().User);
            else if (col.gameObject.GetComponent<Throwable>() != null)
            {
                ThrowableInstance = col.gameObject.GetComponent<Throwable>();
                _parryOccured.Raise(col.gameObject.GetComponent<Throwable>().User);
            }
            
            Destroy(this);
            
        }
    }
}