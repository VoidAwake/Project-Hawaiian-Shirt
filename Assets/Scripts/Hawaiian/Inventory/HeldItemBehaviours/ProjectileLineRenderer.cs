using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hawaiian.Inventory.HeldItemBehaviours
{
    public class ProjectileLineRenderer : MonoBehaviour
    {
        private static readonly int Rate = Shader.PropertyToID("_Rate");
        
        [SerializeField] private ItemShoot itemShoot;

        private readonly List<LineRenderer> lineRenderers = new();

        private void OnEnable()
        {
            itemShoot.targetCountChanged.AddListener(OnTargetCountChanged);
            itemShoot.multiShotTargetsUpdated.AddListener(OnMultiShotTargetsUpdated);
        }

        private void OnDisable()
        {
            itemShoot.targetCountChanged.RemoveListener(OnTargetCountChanged);
            itemShoot.multiShotTargetsUpdated.RemoveListener(OnMultiShotTargetsUpdated);
        }

        private void OnTargetCountChanged()
        {
            GenerateLineRenderers();
        }

        private void OnMultiShotTargetsUpdated()
        {
            UpdateLineRenderers(); 
        }

        private void GenerateLineRenderers()
        {
            DestroyLineRenderers();

            for (int i = 0; i < (itemShoot.Item.ProjectileAmount == 0 ? 1 : itemShoot.Item.ProjectileAmount); i++)
            {
                GameObject instance = new GameObject();

                instance.transform.parent = transform.parent;

                LineRenderer renderer = instance.AddComponent(typeof(LineRenderer)) as LineRenderer;
                renderer.sortingOrder = 100;
                lineRenderers.Add(renderer);

                renderer.material = Resources.Load<Material>("Sprites/lineRendererMaterial");

                renderer.material.SetFloat(Rate, renderer.material.GetFloat(Rate) * -1);

                renderer.startWidth = 0.2f;
                renderer.endWidth = 0.2f;

                Gradient gradient = new Gradient();
                gradient.SetKeys(
                    new GradientColorKey[]
                    {
                        new GradientColorKey(itemShoot.UnitPlayer.PlayerColour, 0.0f),
                        new GradientColorKey(itemShoot.UnitPlayer.PlayerColour, 1.0f)
                    }, new GradientAlphaKey[] {new GradientAlphaKey(1, 0.0f), new GradientAlphaKey(1, 1.0f)});

                renderer.colorGradient = gradient;
            }
        }

        private void DestroyLineRenderers()
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
        }

        private void UpdateLineRenderers()
        {
            GenerateLineRenderers();
            for (var i = 0; i < itemShoot._multiShotTargets.Length; i++)
            {
                LineRenderer lr = lineRenderers[i];
                lr.transform.localPosition = Vector3.zero;

                Vector3[] otherPositions = {itemShoot._multiShotTargets[i], itemShoot.UnitPlayer.transform.position + Vector3.up * 0.5f};

                lr.positionCount = 2;
                lr.SetPositions(otherPositions);
            }
        }

        private void Update()
        {
            foreach (var lineRenderer in lineRenderers)
            {
                lineRenderer.enabled = itemShoot.UseItemActionHeld;
            }
        }

        private void OnDestroy()
        {
            DestroyLineRenderers();
        }
    }
}