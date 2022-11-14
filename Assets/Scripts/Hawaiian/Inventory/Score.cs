using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Hawaiian.Inventory
{
    public class Score : MonoBehaviour
    {
        [SerializeField] private TreasureDic dictionary;
        
        public int ScoreValue
        {
            get => scoreValue;
            set
            {
                scoreValue = value;
                scoreChanged.Invoke();
            }
        }

        [NonSerialized] public UnityEvent scoreChanged = new();
        
        private int scoreValue;
        
        public void DropCash()
        {
            //List<int> toSpawn = new List<int>();
            //toSpawn = GetTreasuresToDrop((int)Mathf.Floor(inv.score * (30 + (inv.score/200 * 50))/100));
            int amountToDrop = ((int) Mathf.Floor(ScoreValue * (30 + (ScoreValue / 200 * 50)) / 100));
            Item[] toSpawn = dictionary.GetTreasuresToDrop(amountToDrop);
            foreach (Item reference in toSpawn)
            {
                Item treasure = ScriptableObject.CreateInstance<Item>();
                treasure.ItemName = reference.ItemName;
                treasure.ItemSprite = reference.ItemSprite;
                treasure.DroppedItemSprite = reference.DroppedItemSprite;
                treasure.Type = ItemType.Objective;
                treasure.Points = reference.Points;
                treasure.DroppedItemBase = reference.DroppedItemBase;

                GameObject droppedTreasure = Instantiate(reference.DroppedItemBase,transform.position, Quaternion.identity);
                droppedTreasure.GetComponent<DroppedItem>().Item = treasure;

                for (int i = 0; i < droppedTreasure.transform.childCount; i++)
                {
                    if (droppedTreasure.transform.GetChild(i).name == "Item Sprite")
                    {
                        droppedTreasure.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite =
                            treasure.ItemSprite;
                    }
                }
                
                droppedTreasure.GetComponent<ItemUnit>().OnThrow(Random.insideUnitCircle);
            }

            LoseTreasure(amountToDrop);
        }

        public void LoseTreasure(int dropped)
        {
            ScoreValue -= dropped;
        }

        public void AddTreasure(Item item)
        {
            if (item.Type == ItemType.Objective)
            {
                ScoreValue += (int)item.Points;
            }
        }
    }
}