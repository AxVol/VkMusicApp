using VKMusicApp.Models;
using VkNet.Model;

namespace VKMusicApp.Services.AudioPlayer.Interfaces
{
    public interface IAudioPlayerService
    {
        public PlayerAudios PlayerAudios { get; set; }

        public string UrlConverter(Uri Url);
        public void SetNextAudio();
        public void SetBackAudio();
        public void SetNewAudio(Audio audio);
    }
}
