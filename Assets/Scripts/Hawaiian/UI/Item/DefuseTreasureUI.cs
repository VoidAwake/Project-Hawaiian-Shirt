using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Codice.Client.BaseCommands;
using Hawaiian.Inventory;
using Hawaiian.UI.Utilities;
using TMPro;
using UI.Core;
using UnityEngine;
using UnityEngine.UI;

public class DefuseTreasureUI : DialogueComponent<PlayerTreasureUI>
{
    //TODO: Definitely a better way to have better defuse animations but someone else gotta figure that out
    [SerializeField] private TextMeshProUGUI _defusingText;
    [SerializeField] private ExtendedSlider _defuserSlider;

    private CancellationTokenSource _sliderToken;
    private Coroutine defusingTextCoroutine;


    protected override void Subscribe()
    {
        dialogue._playerTreasure.OnDefuseInitiated += OnDefuseStarted;
        dialogue._playerTreasure.OnDefusedInterrupted += OnDefuserInterrupted;
        dialogue._playerTreasure.OnDefuseCompleted += OnDefuseCompleted;
    }

    protected override void Unsubscribe()
    {
        dialogue._playerTreasure.OnDefuseInitiated -= OnDefuseStarted;
        dialogue._playerTreasure.OnDefusedInterrupted -= OnDefuserInterrupted;
        dialogue._playerTreasure.OnDefuseCompleted -= OnDefuseCompleted;
    }

    protected override void OnComponentStart()
    {
        base.OnComponentStart();
        _defuserSlider.Hide();
        _defusingText.gameObject.SetActive(false);
    }

    private async void OnDefuseCompleted()
    {
        if (defusingTextCoroutine != null)
            StopCoroutine(defusingTextCoroutine);

        _defusingText.text = $"Defused!";
        Hide();
        _defuserSlider.StopSliderAnim(_sliderToken, true);
    }

    private void OnDefuserInterrupted()
    {
        if (defusingTextCoroutine != null)
            StopCoroutine(defusingTextCoroutine);

        Hide();
        _defuserSlider.StopSliderAnim(_sliderToken, true);
    }

    private async void OnDefuseStarted()
    {
        _sliderToken = new CancellationTokenSource();
        _defuserSlider.Show();
        _defusingText.gameObject.SetActive(true);
        defusingTextCoroutine = StartCoroutine(DefusingTextAnimationCoroutine());

        try
        {
            await _defuserSlider.BeginSliderAnim(dialogue._playerTreasure.DefuseTimer, _sliderToken.Token);
        }
        catch (OperationCanceledException)
        {
            Debug.Log($"Defusing was Interrupted!");
        }
    }

    private void Hide()
    {
        _defuserSlider.Hide();
        _defusingText.gameObject.SetActive(false);
        _defuserSlider.SetCurrentValue(0);
    }


    private IEnumerator DefusingTextAnimationCoroutine()
    {
        while (true)
        {
            _defusingText.text = $"Defusing";
            yield return new WaitForSeconds(0.5f);

            for (int i = 0; i < 3; i++)
            {
                _defusingText.text += ".";
                yield return new WaitForSeconds(0.5f);
            }

            yield return null;
        }
    }
}