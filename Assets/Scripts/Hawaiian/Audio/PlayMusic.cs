using UnityEngine;

namespace Hawaiian.Audio
{
    public class PlayMusic : AudioBehaviour
    {
        public override void TriggerSound()
        {
            CurrentInstance = overrideVolume
                ? audioManager.PlayMusic(CurrentTrack.Path)
                : audioManager.PlayMusic(CurrentTrack.Path, volume);
        }

        public override void StopSound() => audioManager.StopSound(CurrentInstance);
    }
}
