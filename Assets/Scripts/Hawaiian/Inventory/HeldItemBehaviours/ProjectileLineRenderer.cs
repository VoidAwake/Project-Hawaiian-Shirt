using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hawaiian.Inventory.HeldItemBehaviours
{
    public class ProjectileLineRenderer : MonoBehaviour
    {
        private static readonly int Rate = Shader.PropertyToID("_Rate");
        
        [SerializeField] private InstantiateProjectileHeldItemBehaviour instantiateProjectileHeldItemBehaviour;

        private readonly List<LineRenderer> lineRenderers = new();

        private void OnEnable()
        {
            instantiateProjectileHeldItemBehaviour.targetCountChanged.AddListener(OnTargetCountChanged);
            instantiateProjectileHeldItemBehaviour.multiShotTargetsUpdated.AddListener(OnMultiShotTargetsUpdated);
        }

        private void OnDisable()
        {
            instantiateProjectileHeldItemBehaviour.targetCountChanged.RemoveListener(OnTargetCountChanged);
            instantiateProjectileHeldItemBehaviour.multiShotTargetsUpdated.RemoveListener(OnMultiShotTargetsUpdated);
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

            for (int i = 0; i < (instantiateProjectileHeldItemBehaviour.Item.ProjectileAmount == 0 ? 1 : instantiateProjectileHeldItemBehaviour.Item.ProjectileAmount); i++)
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
                        new(instantiateProjectileHeldItemBehaviour.UnitPlayer.PlayerColour, 0.0f),
                        new(instantiateProjectileHeldItemBehaviour.UnitPlayer.PlayerColour, 1.0f)
                    }, new GradientAlphaKey[] {new(1, 0.0f), new(1, 1.0f)});

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
            for (var i = 0; i < instantiateProjectileHeldItemBehaviour._multiShotTargets.Length; i++)
            {
                LineRenderer lr = lineRenderers[i];
                lr.transform.localPosition = Vector3.zero;

                Vector3[] otherPositions = {instantiateProjectileHeldItemBehaviour._multiShotTargets[i], instantiateProjectileHeldItemBehaviour.UnitPlayer.transform.position + Vector3.up * 0.5f};

                lr.positionCount = 2;
                lr.SetPositions(otherPositions);
            }
        }

        private void Update()
        {
            foreach (var lineRenderer in lineRenderers)
            {
                lineRenderer.enabled = instantiateProjectileHeldItemBehaviour.UseItemActionHeld;
            }
        }

        private void OnDestroy()
        {
            DestroyLineRenderers();
        }
    }
}