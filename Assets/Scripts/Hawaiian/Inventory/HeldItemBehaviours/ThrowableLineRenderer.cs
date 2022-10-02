using System.Linq;
using UnityEngine;

namespace Hawaiian.Inventory.HeldItemBehaviours
{
    public class ThrowableLineRenderer : MonoBehaviour
    {
        [SerializeField] private InstantiateThrowableHeldItemBehaviour instantiateThrowableHeldItemBehaviour;

        [SerializeField] private LineRenderer throwableLineRenderer;

        private void OnEnable()
        {
            instantiateThrowableHeldItemBehaviour.throwableArcPositionsUpdated.AddListener(OnThrowableArcPositionsUpdated);
        }

        private void OnDisable()
        {
            instantiateThrowableHeldItemBehaviour.throwableArcPositionsUpdated.RemoveListener(OnThrowableArcPositionsUpdated);
        }

        private void OnThrowableArcPositionsUpdated()
        {
            UpdateThrowableLineRenderer();
        }

        private void UpdateThrowableLineRenderer()
        {
            throwableLineRenderer.positionCount = instantiateThrowableHeldItemBehaviour.throwableArcPositions.Count;
            throwableLineRenderer.SetPositions(instantiateThrowableHeldItemBehaviour.throwableArcPositions.Select(p => (Vector3) p).ToArray());

            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[]
                {
                    new GradientColorKey(instantiateThrowableHeldItemBehaviour.UnitPlayer.PlayerColour, 0.0f),
                    new GradientColorKey(instantiateThrowableHeldItemBehaviour.UnitPlayer.PlayerColour, 1.0f)
                }, new GradientAlphaKey[] {new GradientAlphaKey(1, 0.0f), new GradientAlphaKey(1, 1.0f)});
            throwableLineRenderer.colorGradient = gradient;
        }

        private void Update()
        {
            throwableLineRenderer.enabled = instantiateThrowableHeldItemBehaviour.UseItemActionHeld;
        }
    }
}