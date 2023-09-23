using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Input;
using VKMusicApp.Models;
using VKMusicApp.Pages;
using VkNet.Model;

namespace VKMusicApp.Core
{
    public class MusicLibrary : ObservableObject
    {
        private ObservableCollection<Audio> viewAudio;

        protected bool searchIsFocus = false;

        public delegate void EntryFocusHandler();

        public event EntryFocusHandler EntryFocus;

        public ICommand UnFocus { get; set; }
        public ICommand SearchFocusCommand { get; set; }
        public ICommand OpenMusicCommand { get; set; }

        public ObservableCollection<Audio> ViewAudio 
        {
            get => viewAudio;
            set
            {
                viewAudio = value;
                OnPropertyChanged();
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

        protected void UnFocused()
        {
            SearchIsFocus = false;
        }

        protected void SearchFocus()
        {
            SearchIsFocus = true;

            EntryFocus.Invoke();
        }

        protected void OpenMusic(object obj)
        {
            Audio audio = obj as Audio;

            PlayerAudios playerAudios = new PlayerAudios()
            {
                PlayingAudio = audio,
                Audios = ViewAudio,
                PathToAudio = UrlConverter(audio.Url)
            };

            Shell.Current.GoToAsync($"{nameof(AudioPlayerPage)}",
                new Dictionary<string, object>
                {
                    ["PlayerAudios"] = playerAudios
                });
        }

        private string UrlConverter(Uri Url)
        {
            string url = Regex.Replace(
                Url.ToString(),
                @"/[a-zA-Z\d]{6,}(/.*?[a-zA-Z\d]+?)/index.m3u8()",
                @"$1$2.mp3");

            return url;
        }
    }
}
