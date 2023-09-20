using System.Collections.ObjectModel;
using System.Windows.Input;
using VkNet.Model;

namespace VKMusicApp.Core
{
    public class MusicLibrary : ObservableObject
    {
        protected bool searchIsFocus = false;

        public delegate void EntryFocusHandler();

        public event EntryFocusHandler EntryFocus;

        public ICommand GoTo { get; set; } = new Command(GoToPage);
        public ICommand UnFocus { get; set; }
        public ICommand SearchFocusCommand { get; set; }

        public ObservableCollection<Audio> ViewAudio { get; set; }

        public bool SearchIsFocus
        {
            get => searchIsFocus;
            set
            {
                searchIsFocus = value;
                OnPropertyChanged();
            }
        }

        protected void UnFocused()
        {
            SearchIsFocus = false;
        }

        protected void SearchFocus()
        {
            SearchIsFocus = true;

            EntryFocus.Invoke();
        }

        private static void GoToPage(object obj)
        {
            string page = obj as string;

            Shell.Current.GoToAsync(page);
        }
    }
}
