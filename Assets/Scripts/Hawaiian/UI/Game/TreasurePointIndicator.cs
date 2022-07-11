using System;
using System.Collections;
using System.Collections.Generic;
using Hawaiian.Inventory;
using Hawaiian.UI.Game;
using Hawaiian.Unit;
using TMPro;
using UI.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class TreasurePointIndicator : DialogueComponent<GameDialogue>
{
    [SerializeField] private TextMeshProUGUI _points;
    [SerializeField] private Image _backgroundColour;
    [SerializeField] private UnitPlayer unitPlayer;

    public Inventory inventory;


    Coroutine textCoroutine;
    int currentScore;
    int targetScore;

    float fontSize;

    public void Initialise(UnitPlayer unitPlayer)
    {
        this.unitPlayer = unitPlayer;
        _backgroundColour.color = unitPlayer.PlayerColour;
    }

    public void UpdatePointIndicator(Tuple<IUnit,int> data)
    {

        if (unitPlayer.GetComponent<IUnit>() != data.Item1)
            return;

        targetScore = data.Item2;
        
        // for (int i = 0; i < playerReference.transform.childCount; i++)
        // {
        //     InventoryController temp = playerReference.transform.GetChild(i)
        //         .GetComponent<InventoryController>();
        //
        //     if (temp == null) continue;
        //
        //     inventory = temp._inv;
        //
        // }
        //
        UpdateText();
    }

    public void UpdatePoints(Tuple<IUnit,int> data)
    {
        
        if (unitPlayer.GetComponent<IUnit>() != data.Item1)
            return;

        targetScore = data.Item2;

        UpdateText();
    }

    protected override void OnComponentStart()
    {
        base.OnComponentStart();

        _points.text = "0";
        fontSize = _points.fontSize;
        UpdateText();
    }


    private void UpdateText()
    {
        //
        // if (inventory == null)
        // {
        //     _points.text = "0";
        //     return;
        // }

        // List<int> indexes = new List<int>();
        //
        // for (var index = 0; index < inventory.inv.Length; index++)
        // {
        //     Item item = inventory.inv[index];
        //
        //     if (item != null)
        //     {
        //         if (item.Type == ItemType.Objective)
        //         {
        //             indexes.Add(index);
        //             targetScore += (int) item.Points;
        //         }
        //
        //     }
        // }
        //
        
        
        // for (var i = 0; i < inventory.inv.Length; i++)
        // {
        //     foreach (var t in indexes)
        //     {
        //         if (t == i)
        //             inventory.RemoveItemAt(i);
        //     }
        // }

        if (textCoroutine != null)
        {
            StopCoroutine(textCoroutine);
        }

        textCoroutine = StartCoroutine(AnimateText());
    }

    private IEnumerator AnimateText()
    {
        bool metTarget = false;
        int rate = 0;

        if (currentScore < targetScore)
        {
            rate = 1;
            //text.rectTransform.localScale = new Vector2(0.85f, 1.0f);
            _points.fontSize = fontSize * 1.2f;
        }
        else if (targetScore < currentScore)
        {
            rate = -1;
            //text.rectTransform.localScale = new Vector2(1.0f, 0.65f);
            _points.fontSize = fontSize * 0.8f;
        }

        while (!metTarget)
        {
            currentScore += rate;
            _points.text = "" + currentScore;

            if (currentScore != targetScore)
            {
                yield return new WaitForSeconds(0.02f);
            }
            else
            {
                metTarget = true;
            }
        }

        //text.rectTransform.localScale = new Vector2(1,1);
        _points.fontSize = fontSize;
        textCoroutine = null;
    }

    protected override void Subscribe()
    {
    }

    protected override void Unsubscribe()
    {
    }
}