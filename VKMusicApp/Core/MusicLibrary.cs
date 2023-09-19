using System.Windows.Input;

namespace VKMusicApp.Core
{
    public class MusicLibrary : ObservableObject
    {
        protected bool searchIsFocus = false;

        public delegate void EntryFocusHandler();
        public event EntryFocusHandler EntryFocus;

        public ICommand GoTo { get; set; } = new Command(GoToPage);
        public ICommand UnFocus { get; set; }
        public ICommand SearchCommand { get; set; }

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

        protected void Search()
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
