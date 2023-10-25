using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using VKMusicApp.Core;
using VKMusicApp.Services.AudioPlayer.Interfaces;
using VKMusicApp.Services.Implementation;
using VKMusicApp.Services.Interfaces;
using VkNet.Model;

namespace VKMusicApp.ViewModels
{
    public partial class AccountMusicViewModel : MusicLibrary
    {
        private readonly IVkService vkService;

        [ObservableProperty]
        private bool viewAudioIsVisable = true;

        [ObservableProperty]
        private bool searchAudioIsVisable = false;

        [ObservableProperty]
        private ObservableCollection<Audio> searchAudio;

        private string searchText;
        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                SortMusic();
                OnPropertyChanged(nameof(SearchText));
            }
        }

        public AccountMusicViewModel(IVkService VkService, IAudioPlayerService service, IFileService file)
        {
            vkService = VkService;
            AudioPlayerService = service;
            FileService = file;

            SearchAudio = new ObservableCollection<Audio>();
            ViewAudio = new ObservableCollection<Audio>(vkService.GetAudios());
        }

        private void SortMusic()
        {
            if (!searchText.Any())
            {
                ViewAudioIsVisable = true;
                SearchAudioIsVisable = false;

                return;
            }

            SearchAudio.Clear();

            ViewAudioIsVisable = false;
            SearchAudioIsVisable = true;
            List<Audio> audios = new List<Audio>();

            _ = Task.Run(() =>
            {
                foreach (Audio audio in ViewAudio)
                {
                    if (audio.Title.ToLower().StartsWith(searchText.ToLower()))
                    {
                        SearchAudio.Add(audio);
                    }
                }
            });
        }
    }
}
