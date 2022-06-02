using System.Collections.Generic;
using System.Collections;
using Hawaiian.UI.CharacterSelect;
using Hawaiian.Unit;
using Hawaiian.Utilities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace Hawaiian.UI.Game
{
    public class InventoryUI : MonoBehaviour
    {
        //[SerializeField] private GameEvent parsed;
        [SerializeField] public Inventory.Inventory inv;
        [SerializeField] private ScriptableInt size;
        [SerializeField] private GameObject horizontalGroup;
        [SerializeField] private RectTransform inventoryBackdrop;
        private List<GameObject> invSlots = new ();
        [SerializeField] GameObject slot;
        [SerializeField] private GameObject highlightPrefab;
        private GameObject highlight;
        private Coroutine highlightCoroutine;
        [SerializeField] private Sprite[] highlightSprites;

        // Updating UI to match player loaded in
        [SerializeField] private Image head;
        [SerializeField] private Image backdrop;
        [SerializeField] private PlayerSprites playerSprites;
        [SerializeField] private PlayerColors playerColors;

        private void Start()
        {
            for (int i = 0; i < size.Value; i++)
            {
                invSlots.Add(Instantiate(slot, transform.position, quaternion.identity, horizontalGroup.transform));
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

            // Resize inventory backdrop image, and reposition inventory so that it is centred
            float length = (70 + 20 + 8) + (128 + 12) * size.Value;
            inventoryBackdrop.sizeDelta = new Vector2(length, inventoryBackdrop.sizeDelta.y);
            transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector2(70.0f - (70.0f + length) / 2.0f, 0.0f);

            highlight = Instantiate(highlightPrefab, invSlots[0].transform.position, quaternion.identity, invSlots[0].transform);
            inv.currentItemChanged.AddListener(UpdateInv);
            
            SetCharacterPortrait();
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
            inv.currentItemChanged.RemoveListener(UpdateInv);
        }
        
        

        public void OnParsed()
        {
            highlight.transform.position = invSlots[inv.InvPosition].transform.position;
            
            // Animate highlight
            if (highlightCoroutine != null)
            {
                StopCoroutine(highlightCoroutine);
            }
            highlightCoroutine = StartCoroutine(AnimateHighlight());
        }


        IEnumerator AnimateHighlight()
        {
            int count = 0;
            Image highlightRenderer = highlight.GetComponent<Image>();

            // Animate highlight
            while (count < 7)
            {
                switch (count) {
                    case 0:
                    case 4:
                        highlightRenderer.sprite = highlightSprites[0];
                        break;
                    case 1:
                    case 3:
                    case 5:
                    case 7:
                        highlightRenderer.sprite = highlightSprites[1];
                        break;
                    case 2:
                    case 6:
                        highlightRenderer.sprite = highlightSprites[2];
                        break;
                    default:
                        highlightRenderer.sprite = highlightSprites[1];
                        break;
                }
                count++;
                yield return new WaitForSeconds(0.05f);
            }

            highlightCoroutine = null;
        }

        private void SetCharacterPortrait()
        {
            var playerManager = FindObjectOfType<PlayerManager>();

            if (playerManager == null) return;

            var playerConfig = playerManager.GetPlayerConfig(inv);

            if (playerConfig == null)
            {
                Debug.LogWarning($"Could not find the {nameof(PlayerConfig)}.");
                return;
            }
            
            head.sprite = playerSprites.GetSprite(playerConfig.characterNumber);
            backdrop.color = playerColors.GetColor(playerConfig.playerNumber);
        }
    }
}
