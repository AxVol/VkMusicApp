using System.Collections.ObjectModel;
using VKMusicApp.Core;
using VKMusicApp.Services;
using VKMusicApp.Services.AudioPlayer.Interfaces;
using VKMusicApp.Services.Interfaces;
using VkNet.Model;

namespace VKMusicApp.ViewModels
{
    public partial class MusicPlaylistViewModel : MusicLibrary
    {
        private readonly PlaylistService playlistService;

        public MusicPlaylistViewModel(IAudioPlayerService service, IFileService file, PlaylistService playlist)
        {
            AudioPlayerService = service;
            FileService = file;
            playlistService = playlist;

            ViewAudio = new ObservableCollection<Audio>(playlistService.Audios);
        }
    }
}
