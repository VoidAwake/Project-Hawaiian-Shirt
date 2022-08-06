using System.Collections;
using UnityEngine;

namespace Hawaiian.Inventory.HeldItemBehaviours
{
    public class SoundEffect : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;

        private bool destroyWhenFinished;
        private Coroutine coroutine;

        // TODO: Causing error because we're also using initalise as a message
        public void Initialise(Destroyable destroyable)
        {
            // TODO: Unlisten
            destroyable.destroyed.AddListener(OnDestroyed);
        }

        private void OnDestroyed()
        {
            if (coroutine == null)
                Destroy(gameObject);
            else
                destroyWhenFinished = true;
        }

        public void PlayOneShot(AudioClip audioClip)
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
            
            coroutine = StartCoroutine(PlayOneShotRoutine(audioClip));
        }

        private IEnumerator PlayOneShotRoutine(AudioClip audioClip)
        {
            audioSource.PlayOneShot(audioClip);

            yield return new WaitForSeconds(audioClip.length);
            
            if (destroyWhenFinished)
                Destroy(gameObject);

            coroutine = null;
        }
    }
}