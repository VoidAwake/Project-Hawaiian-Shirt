using System;
using Hawaiian.Input;
using UnityEngine.InputSystem;
using System.Collections;
using Codice.Client.Common.GameUI;
using Hawaiian.UI;
using Hawaiian.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hawaiian.Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [FormerlySerializedAs("canvas")] [SerializeField] private Transform uiParent;
        [SerializeField] private GameObject ui;
        [SerializeField] private Item item;
        [SerializeField] private GameObject highlight;
        [SerializeField] private SpriteRenderer handheld;
        [SerializeField] private int invPosition = 0;
        [SerializeField] private bool addinv;
        [SerializeField] private GameEventListeners _listener;
        [SerializeField] private Item[] _baseItems;
        
        private GameObject reference;
        private Inventory _inv;
        private GameObject highRef;
        private PlayerAction play;
        private bool noDoubleDipping;

        public InventoryUI tempRef;

        //[SerializeField] private int invSize;
        //[SerializeField] private Sprite hand;

        private void Awake()
        {
            // TODO: Temp fix to get canvas reference. Will come back to this when we start looking at a UI system.
            if (uiParent == null) uiParent = FindObjectOfType<Canvas>().transform.GetChild(0);
        
            _inv = ScriptableObject.CreateInstance<Inventory>();
            reference = Instantiate(ui, uiParent);
            tempRef = reference.GetComponent<InventoryUI>();
            reference.GetComponent<InventoryUI>().inv = _inv;
            //addinv = true;
            noDoubleDipping = true;
            highRef = Instantiate(highlight, uiParent);
            //IFuckingHateThis();
            //yield WaitForSeconds(0);
            //SelectionUpdate();
            play = new PlayerAction();
           // SelectionUpdate();
        }

        public void InitialiseHighlight()
        {
            highRef.transform.position = tempRef.invSlots[invPosition].transform.position;
            

        }

        private void Start()
        {

        }

        private void OnEnable()
        {
            play.Enable();
        }

        private void Update()
        {
            noDoubleDipping = true;
            if (addinv)
            {
                SelectionUpdate();
                addinv = false;
            }

            //float x = play.Player.InventoryParse.ReadValue<float>();
            //Debug.Log(x);
            //bool a = play.Player.InvLeft.triggered;
            if (play.Player.InvParse.triggered)
            {
                Parse((int) play.Player.InvParse.ReadValue<float>());
            }


        }

        private void OnDisable()
        {
            play.Disable();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.tag == "Item" && noDoubleDipping)
            {
                noDoubleDipping = false;
                if(_inv.PickUp(col.gameObject.GetComponent<tempItem>().item))
                {
                    Destroy(col.gameObject);
                    Debug.Log("Hit");
                    SelectionUpdate();
                }
            }
        }

        private void Parse(int i)
        {
            invPosition += i;
            if (invPosition > _inv.inv.Length - 1)
            {
                invPosition = 0;
            }

            if (invPosition < 0)
            {
                invPosition = _inv.inv.Length - 1;
            }
            SelectionUpdate();
            
        }

        public void SelectionUpdate()
        {
            highRef.transform.position = tempRef.invSlots[invPosition].transform.position;
            if (_inv.inv[invPosition] != null)
            {
                handheld.sprite = _inv.inv[invPosition].ItemSprite;
            }
            else
            {
                handheld.sprite = null;
            }
        }
    }
}
