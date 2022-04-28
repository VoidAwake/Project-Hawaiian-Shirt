using System.Collections.Generic;
using Hawaiian.Utilities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

namespace Hawaiian.UI.Game
{
    public class InventoryUI : MonoBehaviour
    {
        //[SerializeField] private GameEvent parsed;
        [SerializeField] public Inventory.Inventory inv;
        [SerializeField] private ScriptableInt size;
        private List<GameObject> invSlots = new ();
        [SerializeField] GameObject slot;
        [SerializeField] private GameObject highlightPrefab;
        private GameObject highlight;

        private void Start()
        {
            for (int i = 0; i < size.Value; i++)
            {
                invSlots.Add(Instantiate(slot, transform.position, quaternion.identity, transform));
                //Instantiate this gamer inventory ui
                if (inv.inv[i] != null)
                {
                    invSlots[i].GetComponent<SlotUI>().UpdateSprite(inv.inv[i].ItemSprite);
                }
                else
                {
                    invSlots[i].GetComponent<SlotUI>().UpdateSprite(null);
                }
                
            }
            highlight = Instantiate(highlightPrefab, invSlots[0].transform.position, quaternion.identity, invSlots[0].transform);
            inv.itemchange.AddListener(UpdateInv);
        }
        

        private void UpdateInv()
        {
            for (int i = 0; i < size.Value; i++)
            {
            
                //invSlots[i].refer to inv
                if (inv.inv[i] != null)
                {
                    invSlots[i].GetComponent<SlotUI>().UpdateSprite(inv.inv[i].ItemSprite);
                }
                else
                {
                    invSlots[i].GetComponent<SlotUI>().UpdateSprite(null);
                }
                //x.sprite = 
            }
        }
    


        private void OnDestroy()
        {
            inv.itemchange.RemoveListener(UpdateInv);
        }
        
        

        public void OnParsed()
        {
            highlight.transform.position = invSlots[inv.invPosition].transform.position;
        }
    }
}
