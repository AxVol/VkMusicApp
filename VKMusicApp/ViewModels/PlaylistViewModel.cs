using System.Collections.ObjectModel;
using System.Windows.Input;
using VKMusicApp.Core;
using VKMusicApp.Pages;
using VKMusicApp.Services.Implementation;
using VkNet.Model;

namespace VKMusicApp.ViewModels
{
    public class PlaylistViewModel : ObservableObject
    {
        private IVkService vkService;

        public ObservableCollection<AudioPlaylist> Playlists { get; set; }

        public ICommand OpenPlaylistCommand { get; set; }

        public PlaylistViewModel(IVkService VkService)
        {
            vkService = VkService;

            OpenPlaylistCommand = new Command(OpenPlayList);

            Playlists = new ObservableCollection<AudioPlaylist>(vkService.GetPlayLists());
        }

        private void OpenPlayList(object obj)
        {
            AudioPlaylist playlist = obj as AudioPlaylist;

            ObservableCollection<Audio> playlistAudio = vkService.GetAudioById((long)playlist.Id);

            Shell.Current.GoToAsync($"{nameof(MusicPlaylistPage)}",
                new Dictionary<string, object>
                {
                    ["ViewAudio"] = playlistAudio
                });
        }
    }
}
