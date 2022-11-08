using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Hawaiian.Inventory;
using Unity.Plastic.Newtonsoft.Json.Bson;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TreasureAnimationController : MonoBehaviour
{
    private static readonly int TreasureOpeningIndex = Animator.StringToHash("Treasure Opening");
    private static readonly int TreasureDepositingIndex = Animator.StringToHash("Treasure Depositing");
    private static readonly int TreasureClosingIndex = Animator.StringToHash("Treasure Closing");
    private static readonly int TreasureDetonatedIndex = Animator.StringToHash("Treasure Detonated");


    private Animator _animator;
    private ParticleSystem _cachedExplosion;

    private bool _treasureOpeningAnimationCompleted = false;
    private bool _treasureDepositingAnimationCompleted = false;
    private bool _treasureClosingAnimationCompleted = false;
    private bool _treasureDetonationAnimationCompleted = false;


    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        _cachedExplosion = GetComponentInChildren<ParticleSystem>();
    }


    //Animation Event Triggers
    public void OpeningCompleted() => _treasureOpeningAnimationCompleted = true;
    public void DepositCompleted() => _treasureDepositingAnimationCompleted = true;
    public void ClosingCompleted() => _treasureClosingAnimationCompleted = true;
    public void DetonationCompleted() => _treasureDetonationAnimationCompleted = true;


    public async UniTask PlayChestOpeningAnim(CancellationToken token)
    {
        try
        {
            _animator.SetTrigger(TreasureOpeningIndex);
            await UniTask.WaitUntil(() => _treasureOpeningAnimationCompleted, PlayerLoopTiming.Update, token);
            _treasureOpeningAnimationCompleted = false;
        } catch (OperationCanceledException e)
        {
            print(e);
        }
    }

    public async UniTask PlayChestDepositAnim(CancellationToken token)
    {
        try
        {
            _animator.SetTrigger(TreasureDepositingIndex);
            await UniTask.WaitUntil(() => _treasureDepositingAnimationCompleted, PlayerLoopTiming.Update, token);
            _treasureDepositingAnimationCompleted = false;
        }
        catch (OperationCanceledException e)
        {
            print(e);
        }
    }

    public async UniTask PlayChestClosingAnim(CancellationToken token)
    {
        try
        {
            _animator.SetTrigger(TreasureClosingIndex);
            await UniTask.WaitUntil(() => _treasureClosingAnimationCompleted, PlayerLoopTiming.Update, token);
            _treasureClosingAnimationCompleted = false;
        }
        catch (OperationCanceledException e)
        {
            _cachedExplosion.Stop();
            print(e);
        }
    }
    
    public async UniTask PlayChestDetonationAnimation(CancellationToken token)
    {
        try
        {
            _cachedExplosion.Play();
            _animator.SetTrigger(TreasureDetonatedIndex);
            await UniTask.WaitUntil(() => _treasureDetonationAnimationCompleted, PlayerLoopTiming.Update, token);
            _treasureDetonationAnimationCompleted = false;
        }
        catch (OperationCanceledException e)
        {
            print(e);
        }
    }

    
}