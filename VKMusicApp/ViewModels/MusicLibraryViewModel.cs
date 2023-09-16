using System.Collections.ObjectModel;
using System.Windows.Input;
using VKMusicApp.Core;
using VkNet;
using VkNet.Model;

namespace VKMusicApp.ViewModels
{
    public class MusicLibraryViewModel : ObservableObject
    {
        private VkApi vkApi;
        private bool searchIsFocus = true;
        private string searchText;
        private bool viewAudioIsVisable = true;
        private bool searchAudioIsVisable = false;

        public delegate void EntryFocusHandler();
        public event EntryFocusHandler EntryFocus;

        public ICommand SearchCommand { get; set; }
        public ICommand UnFocus { get; set; }

        public ObservableCollection<Audio> ViewAudio { get; set; }
        public ObservableCollection<Audio> SearchAudio { get; set; }

        public bool SearchIsFocus
        {
            get => searchIsFocus;
            set
            {
                searchIsFocus = value;
                OnPropertyChanged();
            }
        }

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

        public MusicLibraryViewModel(VkApi VKApi)
        {
            vkApi = VKApi;

            var music = vkApi.Audio.Get(new AudioGetParams()
            {
                OwnerId = vkApi.UserId
            }).ToList();

            SearchCommand = new Command(Search);
            UnFocus = new Command(UnFocused);

            SearchAudio = new ObservableCollection<Audio>();
            ViewAudio = new ObservableCollection<Audio>(music);
        }

        public void UnFocused()
        {
            SearchIsFocus = false;
        }

        private void Search()
        {
            SearchIsFocus = true;

            EntryFocus.Invoke();
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
