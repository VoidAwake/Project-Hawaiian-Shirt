using System.Collections.Generic;
using Hawaiian.UI;
using Unity.Mathematics;
using UnityEngine;

namespace Hawaiian.Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] public Inventory inv;
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
