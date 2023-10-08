using System.Collections.ObjectModel;
using VKMusicApp.Core;
using VKMusicApp.Services.AudioPlayer.Interfaces;
using VkNet.Model;

namespace VKMusicApp.ViewModels
{
    [QueryProperty(nameof(ViewAudio), nameof(ViewAudio))]
    public class MusicPlaylistViewModel : MusicLibrary
    {
        public MusicPlaylistViewModel(IAudioPlayerService service)
        {
            AudioPlayerService = service;

            ViewAudio = new ObservableCollection<Audio>();

            OpenMusicCommand = new Command(OpenMusic);
        }
    }
}
