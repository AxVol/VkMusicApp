using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using VKMusicApp.Core;
using VKMusicApp.Services.AudioPlayer.Interfaces;
using VKMusicApp.Services.Interfaces;
using VkNet.Model;

namespace VKMusicApp.ViewModels
{
    public partial class PhoneMusicViewModel : MusicLibrary
    {
        [ObservableProperty]
        private bool viewAudioIsVisable = true;

        [ObservableProperty]
        private bool searchAudioIsVisable = false;

        [ObservableProperty]
        private bool hasEthernet;

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

        public PhoneMusicViewModel(IFileService file, IAudioPlayerService service)
        {
            FileService = file;
            AudioPlayerService = service;

            HasEthernet = LoginViewModel.HasEthernet();

            SearchAudio = new ObservableCollection<Audio>();
            ViewAudio = new ObservableCollection<Audio>(FileService.GetMusics());

            FileService.AudioDownloaded += AudioDownload;
            FileService.AudioDeleted += AudioDelete;

            if (!HasEthernet)
            {
                NavigationBackground = Colors.Gray;
            }
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

        private void AudioDownload(Audio audio)
        {
            ViewAudio.Add(audio);
        }

        private void AudioDelete(Audio audio)
        {
            ViewAudio.Remove(audio);
        }
    }
}
