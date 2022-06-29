using System.Collections;
using Hawaiian.Inventory;
using Hawaiian.UI.Game;
using MoreLinq;
using TMPro;
using UI.Core;
using UnityEngine;
using UnityEngine.InputSystem;

public class TreasureHoardUI : DialogueComponent<GameDialogue>
{
    [SerializeField] private GameObject TreasureHoardUIPrefab;
    [SerializeField] private Transform TreasureHoardUIParent;




    // float timer;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Teasure hoard UI placed");
    }




    public void GenerateTreasurePointUI(PlayerInput playerReference, Color32 playerColour)
    {
        GameObject treasurePrefab = Instantiate(TreasureHoardUIPrefab, TreasureHoardUIParent);
        treasurePrefab.GetComponent<TreasurePointIndicator>().Initialise(playerReference, playerColour);
    }

 

    protected override void Subscribe()
    {
    }

    protected override void Unsubscribe()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}