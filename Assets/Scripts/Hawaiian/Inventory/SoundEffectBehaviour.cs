using Hawaiian.Inventory.HeldItemBehaviours;
using UnityEngine;

namespace Hawaiian.Inventory
{
    public class SoundEffectBehaviour : MonoBehaviour
    {
        [SerializeField] private AudioClip audioClip;
        [SerializeField] private GameObject soundEffectPrefab;
        [SerializeField] private Destroyable destroyable;

        private SoundEffect soundEffect;

        private void Awake()
        {
            // TODO: Where can we instantiate it so it will always be safe?
            var soundEffectObject = Instantiate(soundEffectPrefab, transform.parent);

            soundEffect = soundEffectObject.GetComponent<SoundEffect>();

            if (soundEffect != null)
            {
                soundEffect.Initialise(destroyable);
            }
        }

        public void Play()
        {
            soundEffect.PlayOneShot(audioClip);
        }
    }
}