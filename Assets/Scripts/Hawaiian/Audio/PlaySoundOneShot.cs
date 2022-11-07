using FMOD.Studio;
using UnityEngine;

namespace Hawaiian.Audio
{
    public class PlaySoundOneShot : AudioBehaviour
    {

        [SerializeField] public AudioManager audioManager;   
    
        private void Awake()
        {
            if (playOnAwake)
                PlaySound();
            
        }
    
        private void OnDestroy()
        {
            if (CurrentInstance.isValid())
                CurrentInstance.stop(STOP_MODE.IMMEDIATE);
        }

        public void PlaySound()
        {
            CurrentInstance = overrideVolume
                ? audioManager.PlaySoundOneShot(CurrentTrack.Path)
                : audioManager.PlaySoundOneShot(CurrentTrack.Path, volume);
        }

  
    }
}
