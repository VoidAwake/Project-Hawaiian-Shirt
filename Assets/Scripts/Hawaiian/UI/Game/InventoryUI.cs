using System.Collections.Generic;
using Hawaiian.Utilities;
using Unity.Mathematics;
using UnityEngine;

namespace Hawaiian.UI.Game
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] public Inventory.Inventory inv;
        [SerializeField] private ScriptableInt size;
        private List<GameObject> invSlots = new ();
        [SerializeField] GameObject slot;

        private void Start()
        {
            for (int i = 0; i < size.value; i++)
            {
                invSlots.Add(Instantiate(slot, transform.position, quaternion.identity, transform));
                //Instantiate this gamer inventory ui
                if (inv.inv[i] != null)
                {
                    invSlots[i].GetComponent<SlotUI>().UpdateSprite(inv.inv[i].itemSprite);
                }
                else
                {
                    invSlots[i].GetComponent<SlotUI>().UpdateSprite(null);
                }
            }
        
            inv.itemchange.AddListener(UpdateInv);
        }

        private void UpdateInv()
        {
            for (int i = 0; i < size.value; i++)
            {
            
                //invSlots[i].refer to inv
                if (inv.inv[i] != null)
                {
                    invSlots[i].GetComponent<SlotUI>().UpdateSprite(inv.inv[i].itemSprite);
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
    }
}
