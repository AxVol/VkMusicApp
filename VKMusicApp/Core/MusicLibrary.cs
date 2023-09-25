using System.Collections.ObjectModel;
using System.Windows.Input;
using VKMusicApp.Models;
using VKMusicApp.Pages;
using VKMusicApp.Services.AudioPlayer.Implementation;
using VKMusicApp.Services.AudioPlayer.Interfaces;
using VkNet.Model;

namespace VKMusicApp.Core
{
    public class MusicLibrary : ObservableObject
    {
        private ObservableCollection<Audio> viewAudio;
        protected IAudioPlayerService audioPlayerService;

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
            CollectionView collectionView = obj as CollectionView;

            Audio audio = collectionView.SelectedItem as Audio;
            ObservableCollection<Audio> audioCollection = new ObservableCollection<Audio>();
            int AudioIndex = 0;

            foreach (var music in collectionView.ItemsSource)
            {
                audioCollection.Add((Audio)music);
            }

            foreach (Audio Audio in audioCollection)
            {
                if (audio.Artist == Audio.Artist && audio.Title == Audio.Title)
                    break;

                AudioIndex++;
            }

            PlayerAudios playerAudios = new PlayerAudios()
            {
                PlayingAudio = audio,
                Audios = audioCollection,
                AudioIndex = AudioIndex
            };

            playerAudios.PathToAudio = audioPlayerService.UrlConverter(audio.Url);

            audioPlayerService.PlayerAudios = playerAudios;

            Shell.Current.GoToAsync(nameof(AudioPlayerPage));
        }
    }
}
