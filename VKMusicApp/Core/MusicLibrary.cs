using System.Collections.ObjectModel;
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
            ObservableCollection<Audio> audios = new ObservableCollection<Audio>();
            CollectionView collectionView = obj as CollectionView;
            Audio audio = collectionView.SelectedItem as Audio;
            int AudioIndex = 0;

            foreach (Audio Audio in collectionView.ItemsSource)
            {
                if (audio.Artist == Audio.Artist && audio.Title == Audio.Title)
                    break;

                AudioIndex++;
            }

            foreach (Audio Audio in collectionView.ItemsSource)
            {
                audios.Add(audio);
            }

            PlayerAudios playerAudios = new PlayerAudios()
            {
                PlayingAudio = audio,
                Audios = audios,
                AudioIndex = AudioIndex
            };

            AudioPlayerService.PlayerAudios = playerAudios;
            playerAudios.PathToAudio = audioPlayerService.UrlConverter(audio.Url);

            if (AudioPlayerService.MusicSet)
            {
                AudioPlayerService.Player.MusicPath = AudioPlayerService.PlayerAudios.PathToAudio;
                AudioPlayerService.Player.PlayerAudios = AudioPlayerService.PlayerAudios;

                if (AudioPlayerService.PlayerAudios.PlayingAudio.Title == audio.Title
                && AudioPlayerService.PlayerAudios.PlayingAudio.Artist == audio.Artist)
                {
                    Shell.Current.GoToAsync(nameof(AudioPlayerPage));

                    return;
                }
            }

            Shell.Current.GoToAsync(nameof(AudioPlayerPage));
        }
    }
}
