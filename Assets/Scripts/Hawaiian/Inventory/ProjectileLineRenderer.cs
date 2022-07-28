using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hawaiian.Inventory
{
    public class ProjectileLineRenderer : MonoBehaviour
    {
        [SerializeField] private ItemInteractor itemInteractor;

        private readonly List<LineRenderer> lineRenderers = new();

        private void OnEnable()
        {
            itemInteractor.targetCountChanged.AddListener(OnTargetCountChanged);
            itemInteractor.multiShotTargetsUpdated.AddListener(OnMultiShotTargetsUpdated);
        }

        private void OnDisable()
        {
            itemInteractor.targetCountChanged.RemoveListener(OnTargetCountChanged);
            itemInteractor.multiShotTargetsUpdated.RemoveListener(OnMultiShotTargetsUpdated);
        }

        private void OnTargetCountChanged()
        {
            if (itemInteractor.CurrentItem == null) return;
            
            if (itemInteractor.CurrentItem.Type != ItemType.Projectile) return;
            
            GenerateLineRenderers();
        }

        private void OnMultiShotTargetsUpdated()
        {
            if (itemInteractor.CurrentItem == null) return;
            
            if (itemInteractor.CurrentItem.Type != ItemType.Projectile) return;
            
            UpdateLineRenderers(); 
        }

        private void GenerateLineRenderers()
        {
            if (lineRenderers.Count > 0)
            {
                for (int i = lineRenderers.Count - 1; i >= 0; i--)
                {
                    LineRenderer lr = lineRenderers[i];
                    lineRenderers.Remove(lineRenderers[i]);
                    Destroy(lr.gameObject);
                }
            }
            
            for (int i = 0; i < (itemInteractor.CurrentItem.ProjectileAmount == 0 ? 1 : itemInteractor.CurrentItem.ProjectileAmount); i++)
            {
                GameObject instance = new GameObject();

                instance.transform.parent = transform.parent;

                LineRenderer renderer = instance.AddComponent(typeof(LineRenderer)) as LineRenderer;
                renderer.sortingOrder = 100;
                lineRenderers.Add(renderer);

                renderer.material = Resources.Load<Material>("Sprites/lineRendererMaterial");

                if (itemInteractor.CurrentItem.Type == ItemType.Projectile)
                    renderer.material.SetFloat(ItemInteractor.Rate, renderer.material.GetFloat(ItemInteractor.Rate) * -1);

                renderer.startWidth = 0.2f;
                renderer.endWidth = 0.2f;

                Gradient gradient = new Gradient();
                gradient.SetKeys(
                    new GradientColorKey[]
                    {
                        new GradientColorKey(itemInteractor._playerReference.PlayerColour, 0.0f),
                        new GradientColorKey(itemInteractor._playerReference.PlayerColour, 1.0f)
                    }, new GradientAlphaKey[] {new GradientAlphaKey(1, 0.0f), new GradientAlphaKey(1, 1.0f)});

                renderer.colorGradient = gradient;
            }
        }
        
        private void UpdateLineRenderers()
        {
            GenerateLineRenderers();
            for (var i = 0; i < itemInteractor._multiShotTargets.Length; i++)
            {
                LineRenderer lr = lineRenderers[i];
                lr.transform.localPosition = Vector3.zero;

                Vector3[] otherPositions = {itemInteractor._multiShotTargets[i], itemInteractor._playerReference.transform.position + Vector3.up * 0.5f};

                lr.positionCount = 2;
                lr.SetPositions(otherPositions);
            }
        }

        private void Update()
        {
            var lineRenderersEnabled =
                itemInteractor._isHoldingAttack &&
                itemInteractor.CurrentItem.Type == ItemType.Projectile;
            
            foreach (var lineRenderer in lineRenderers)
            {
                lineRenderer.enabled = lineRenderersEnabled;
            }
        }
    }
}