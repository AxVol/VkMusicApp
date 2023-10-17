using ProtoBuf.Meta;
using System.Collections.ObjectModel;
using VKMusicApp.Core;
using VKMusicApp.Services.AudioPlayer.Interfaces;
using VKMusicApp.Services.Interfaces;
using VkNet.Model;

namespace VKMusicApp.ViewModels
{
    public class PhoneMusicViewModel : MusicLibrary
    {
        private string searchText;
        private bool viewAudioIsVisable = true;
        private bool searchAudioIsVisable = false;

        public ObservableCollection<Audio> SearchAudio { get; set; }

        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                SortMusic();
                OnPropertyChanged();
            }
        }

        public bool ViewAudioIsVisable
        {
            get => viewAudioIsVisable;
            set
            {
                viewAudioIsVisable = value;
                OnPropertyChanged();
            }
        }

        public bool SearchAudioIsVisable
        {
            get => searchAudioIsVisable;
            set
            {
                searchAudioIsVisable = value;
                OnPropertyChanged();
            }
        }

        public PhoneMusicViewModel(IFileService file, IAudioPlayerService service)
        {
            fileService = file;
            AudioPlayerService = service;

            UnFocus = new Command(UnFocused);
            SearchFocusCommand = new Command(SearchFocus);
            OpenMusicCommand = new Command(OpenMusic);
            ShowPopUpCommand = new Command(ShowPopUp);

            SearchAudio = new ObservableCollection<Audio>();
            ViewAudio = new ObservableCollection<Audio>(fileService.GetMusics());
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
    }
}
