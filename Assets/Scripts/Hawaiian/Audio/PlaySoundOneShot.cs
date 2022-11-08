using FMOD.Studio;
using UnityEngine;

namespace Hawaiian.Audio
{
    public class PlaySoundOneShot : AudioBehaviour
    {
        private void PlaySound()
        {
            CurrentInstance = overrideVolume
                ? audioManager.PlaySoundOneShot(CurrentTrack.Path)
                : audioManager.PlaySoundOneShot(CurrentTrack.Path, volume);
        }

        public override void TriggerSound() => PlaySound();

        public override void StopSound() => audioManager.StopSound(CurrentInstance);
    }
}