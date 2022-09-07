using Hawaiian.Inventory;
using Hawaiian.Unit;
using TMPro;
using UnityEngine;

public class ItemText : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private InventoryController _controller;
    [SerializeField] private TextMeshPro text;
    [SerializeField] private float duration;
    [SerializeField] private float speed;
    [SerializeField] private Color color;
    [SerializeField] private PlayerColors options;
    public Color fade;
    private float timer;

    void Start()
    {
        //_controller = GetComponent<InventoryController>();
        _controller.inventoryChanged.AddListener(OnCurrentItemChanged);
        color = new Color(1,1,1,1); //options.GetColor(_controller._player.PlayerNumber);
        fade = color;
        fade = new Color(color.r, color.g, color.b, 0);
        text.color = fade;
        text.text = "";
    }

    private void OnCurrentItemChanged()
    {
        if (_controller.inv.inv[_controller.inv.invPosition] != null)
        {
            if (text.text != _controller.inv.inv[_controller.inv.invPosition].ItemName)
            {
                text.text = _controller.inv.inv[_controller.inv.invPosition].ItemName;
                text.color = color;
                timer = duration;
                Debug.Log(_controller.inv.inv[_controller.inv.invPosition].ItemName);
            }
        }
        else
        {
            text.text = "";
        }
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            text.color = Color.Lerp(text.color, fade, speed * Time.deltaTime);
        }
    }
}