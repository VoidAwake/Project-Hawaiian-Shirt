using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Hawaiian.Inventory
{
    [CreateAssetMenu]
    public class Inventory : ScriptableObject
    {
        [SerializeField] public Item[] inv;
        [SerializeField] public int size;
        public UnityEvent currentItemChanged = new();
        public int invPosition = 0;
        public int score = 0;

        public int InvPosition
        {
            get => invPosition;
            set => invPosition = value;
        }

        public Item CurrentItem => inv[InvPosition];
        
        //public float Score => inv.Where(i => i != null).Sum(i => i.Points);
        public float Score => score;

        public void SetInventory(int invSize)
        {
            size = invSize;
            inv = new Item[size];
        }
        
        public bool AddItem(Item item)
        {
            if (item.Type == ItemType.Objective)
            {
                score += (int)item.Points;
                currentItemChanged.Invoke();
                Debug.Log("score is " + score);
                return true;
                
            }
            else
            {
                for (int i = 0; i < inv.Length; i++)
                {
                    if (inv[i] != null) continue;

                    inv[i] = item;


                    currentItemChanged.Invoke();
                    return true;
                }
            }

            return false;
        }

        public void RemoveItemAt(int invPosition)
        {
            inv[invPosition] = null;
            currentItemChanged.Invoke();
        }
    }
}