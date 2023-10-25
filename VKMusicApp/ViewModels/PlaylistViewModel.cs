using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using VKMusicApp.Core;
using VKMusicApp.Pages;
using VKMusicApp.Services;
using VKMusicApp.Services.AudioPlayer.Interfaces;
using VKMusicApp.Services.Implementation;
using VkNet.Model;

namespace VKMusicApp.ViewModels
{
    public partial class PlaylistViewModel : BaseViewModel
    {
        private readonly IVkService vkService;
        private readonly PlaylistService playlistService;

        [ObservableProperty]
        private ObservableCollection<AudioPlaylist> playlists;

        public PlaylistViewModel(IVkService VkService, IAudioPlayerService service, PlaylistService playlist)
        {
            vkService = VkService;
            AudioPlayerService = service;
            playlistService = playlist;

            Playlists = new ObservableCollection<AudioPlaylist>(vkService.GetPlayLists());
        }

        [RelayCommand]
        private void OpenPlaylist(object obj)
        {
            AudioPlaylist playlist = obj as AudioPlaylist;

            ObservableCollection<Audio> playlistAudio = vkService.GetAudioById((long)playlist.Id);
            playlistService.Audios = playlistAudio.ToList();

            Shell.Current.GoToAsync(nameof(MusicPlaylistPage));
        }
    }
}
