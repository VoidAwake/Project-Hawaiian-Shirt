using FMOD.Studio;
using UnityEngine;

namespace Hawaiian.Audio
{
    public class PlaySoundOneShot : AudioBehaviour
    {
        private void PlaySound()
        {
            CurrentInstance = overrideVolume
                ? audioPlayer.PlaySoundOneShot(CurrentTrack.Path)
                : audioPlayer.PlaySoundOneShot(CurrentTrack.Path, volume);
        }

        public override void TriggerSound() => PlaySound();

        public override void StopSound() => audioPlayer.StopSound(CurrentInstance);
    }
}