using VKMusicApp.Models;
using VKMusicApp.ViewModels;
using VkNet.Model;

namespace VKMusicApp.Services.AudioPlayer.Interfaces
{
    public interface IAudioPlayerService
    {
        public PlayerAudios PlayerAudios { get; set; }
        public bool IsOnline { get; set; }
        public bool MusicSet { get; set; }
        public AudioPlayerViewModel Player { get; set; }

        public string UrlConverter(Uri Url, string action = null);
        public void SetNextAudio();
        public void SetBackAudio();
        public void SetNewAudio(Audio audio);
    }
}
