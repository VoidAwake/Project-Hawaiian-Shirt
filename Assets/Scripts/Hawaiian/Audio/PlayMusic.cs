using UnityEngine;

namespace Hawaiian.Audio
{
    public class PlayMusic : AudioBehaviour
    {
        public override void TriggerSound()
        {
            CurrentInstance = overrideVolume
                ? audioPlayer.PlayMusic(CurrentTrack.Path)
                : audioPlayer.PlayMusic(CurrentTrack.Path, volume);
        }

        public override void StopSound() => audioPlayer.StopSound(CurrentInstance);
    }
}
