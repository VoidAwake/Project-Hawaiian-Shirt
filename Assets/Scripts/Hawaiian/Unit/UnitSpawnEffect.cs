using System.Collections;
using UnityEngine;

namespace Hawaiian.Unit
{
    public class UnitSpawnEffect : MonoBehaviour
    {
        [SerializeField] private UnitPlayer unit;
        [SerializeField] PlayerColors playerColors;
        [SerializeField] private GameObject _spawnEffectPrefab;
        
        private void OnEnable()
        {
            unit.initialised.AddListener(OnInitialised);
        }

        private void OnDisable()
        {
            unit.initialised.RemoveListener(OnInitialised);
        }

        private void OnInitialised()
        {
            //Generate SpawnEffect on player spawn location
            GameObject spawnEffect = Instantiate(_spawnEffectPrefab, unit.transform);
            spawnEffect.GetComponent<AttachGameObjectsToParticles>().LightColour = playerColors.GetColor(unit.PlayerNumber);
            
            ParticleSystem.MainModule settings = spawnEffect.GetComponent<ParticleSystem>().main;
            settings.startColor = new ParticleSystem.MinMaxGradient(playerColors.GetColor(unit.PlayerNumber));
    
        
            //Apply the appropriate material to use the dissolve effect
            UnitAnimator animator = gameObject.GetComponent<UnitAnimator>();
            

            Material dissolveMaterial  = Resources.Load<Material>($"Materials/Player{unit.PlayerNumber}Dissolve");
            foreach (SpriteRenderer renderer in animator.Renderers)
                renderer.material = dissolveMaterial;

            StartCoroutine(RunDissolveCoroutine(dissolveMaterial));
        }
        
        public IEnumerator RunDissolveCoroutine(Material reference)
        {
            var elapsedTime = 0f;
            var endTime = 1f;

            while (elapsedTime < endTime)
            {
                reference.SetFloat("_Amount",Mathf.Lerp(elapsedTime,endTime,elapsedTime/endTime));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}