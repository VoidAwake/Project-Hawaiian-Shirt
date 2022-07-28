using System;
using System.Linq;
using UnityEngine;

namespace Hawaiian.Inventory
{
    public class ThrowableLineRenderer : MonoBehaviour
    {
        [SerializeField] private ItemInteractor itemInteractor;

        [SerializeField] private LineRenderer throwableLineRenderer;

        private void OnEnable()
        {
            itemInteractor.throwableArcPositionsUpdated.AddListener(OnThrowableArcPositionsUpdated);
        }

        private void OnDisable()
        {
            itemInteractor.throwableArcPositionsUpdated.RemoveListener(OnThrowableArcPositionsUpdated);
        }

        private void OnThrowableArcPositionsUpdated()
        {
            if (itemInteractor.CurrentItem == null) return;
            
            if (itemInteractor.CurrentItem.Type != ItemType.Throwable) return;
            
            UpdateThrowableLineRenderer();
        }

        private void UpdateThrowableLineRenderer()
        {
            throwableLineRenderer.positionCount = itemInteractor.throwableArcPositions.Count;
            throwableLineRenderer.SetPositions(itemInteractor.throwableArcPositions.Select(p => (Vector3) p).ToArray());

            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[]
                {
                    new GradientColorKey(itemInteractor._playerReference.PlayerColour, 0.0f),
                    new GradientColorKey(itemInteractor._playerReference.PlayerColour, 1.0f)
                }, new GradientAlphaKey[] {new GradientAlphaKey(1, 0.0f), new GradientAlphaKey(1, 1.0f)});
            throwableLineRenderer.colorGradient = gradient;
        }

        private void Update()
        {
            var throwableLineRendererEnabled =
                itemInteractor._isHoldingAttack &&
                itemInteractor.CurrentItem.Type == ItemType.Throwable;

            throwableLineRenderer.enabled = throwableLineRendererEnabled;
        }
    }
}