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
        private readonly List<Audio> audios;
        private ObservableCollection<Audio> viewAudio;

        public delegate void EntryFocusHandler();
        public event EntryFocusHandler EntryFocus;

        public ICommand SearchCommand { get; set; }
        public ICommand UnFocus { get; set; }

        public ObservableCollection<Audio> ViewAudio 
        { 
            get => viewAudio; 
            set
            {
                viewAudio = value;
                OnPropertyChanged(nameof(ViewAudio));
            }
        }

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

        public MusicLibraryViewModel(VkApi VKApi)
        {
            vkApi = VKApi;

            var music = vkApi.Audio.Get(new AudioGetParams()
            {
                OwnerId = vkApi.UserId
            }).ToList();

            SearchCommand = new Command(Search);
            UnFocus = new Command(UnFocused);

            audios = new List<Audio>(music);
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
            ViewAudio.Clear();

            _ = Task.Run(() =>
            {
                if (!searchText.Any())
                {
                    List<Audio> tmp = new List<Audio>(audios);

                    ViewAudio = new ObservableCollection<Audio>(tmp);

                    return;
                }

                foreach (Audio audio in audios)
                {
                    if (audio.Title.ToLower().StartsWith(searchText.ToLower()))
                    {
                        ViewAudio.Add(audio);
                    }
                }
            });
        }
    }
}
