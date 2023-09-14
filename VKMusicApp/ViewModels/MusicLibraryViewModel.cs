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
        private bool searchIsFocus = false;
        private string searchText;

        public delegate void EntryFocusHandler();
        public event EntryFocusHandler EntryFocus;

        public ICommand SearchCommand { get; set; }
        public ICommand UnFocus {  get; set; }

        public ObservableCollection<Audio> Audios { get; set; }
        public ObservableCollection<Audio> ViewAudio {  get; set; }

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
                OnPropertyChanged();

                SortMusic();
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

            Audios = new ObservableCollection<Audio>(music);
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

            foreach (var audio in Audios)
            {
                if (audio.Title.ToLower().StartsWith(searchText.ToLower()))
                {
                    ViewAudio.Add(audio);
                }
            }
        }
    }
}
