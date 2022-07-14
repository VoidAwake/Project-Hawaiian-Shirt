using System.Collections;
using Hawaiian.Inventory;
using Hawaiian.UI.Game;
using TMPro;
using UI.Core;
using UnityEngine;
using UnityEngine.UI;

public class TreasurePointIndicator : DialogueComponent<GameDialogue>
{
    [SerializeField] private TextMeshProUGUI _points;
    [SerializeField] private Image _backgroundColour;
    [SerializeField] private PlayerTreasure playerTreasure;

    Coroutine textCoroutine;
    int currentScore;
    int targetScore;

    float fontSize;

    private void OnDestroy()
    {
        if (playerTreasure != null)
            playerTreasure.pointsChanged.RemoveListener(OnPointsChanged);
    }

    public void Initialise(PlayerTreasure playerTreasure)
    {
        this.playerTreasure = playerTreasure;
        // TODO: Work out how to get this 
        // _backgroundColour.color = playerTreasure.PlayerColour;
        
        playerTreasure.pointsChanged.AddListener(OnPointsChanged);
    }

    private void OnPointsChanged()
    {
        UpdatePoints(playerTreasure.CurrentPoints);
    }

    public void UpdatePoints(int points)
    {
        targetScore = points;
        
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

    protected override void Subscribe() { }

    protected override void Unsubscribe() { }
}