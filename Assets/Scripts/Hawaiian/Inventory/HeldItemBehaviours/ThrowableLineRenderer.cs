using System.Linq;
using UnityEngine;

namespace Hawaiian.Inventory.HeldItemBehaviours
{
    public class ThrowableLineRenderer : MonoBehaviour
    {
        [SerializeField] private ItemThrow itemThrow;

        [SerializeField] private LineRenderer throwableLineRenderer;

        private void OnEnable()
        {
            itemThrow.throwableArcPositionsUpdated.AddListener(OnThrowableArcPositionsUpdated);
        }

        private void OnDisable()
        {
            itemThrow.throwableArcPositionsUpdated.RemoveListener(OnThrowableArcPositionsUpdated);
        }

        private void OnThrowableArcPositionsUpdated()
        {
            UpdateThrowableLineRenderer();
        }

        private void UpdateThrowableLineRenderer()
        {
            throwableLineRenderer.positionCount = itemThrow.throwableArcPositions.Count;
            throwableLineRenderer.SetPositions(itemThrow.throwableArcPositions.Select(p => (Vector3) p).ToArray());

            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[]
                {
                    new GradientColorKey(itemThrow.UnitPlayer.PlayerColour, 0.0f),
                    new GradientColorKey(itemThrow.UnitPlayer.PlayerColour, 1.0f)
                }, new GradientAlphaKey[] {new GradientAlphaKey(1, 0.0f), new GradientAlphaKey(1, 1.0f)});
            throwableLineRenderer.colorGradient = gradient;
        }

        private void Update()
        {
            throwableLineRenderer.enabled = itemThrow.UseItemActionHeld;
        }
    }
}