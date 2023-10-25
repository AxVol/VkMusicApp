using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using VKMusicApp.Models;
using VKMusicApp.Pages;
using VkNet.Model;

namespace VKMusicApp.Core
{
    public partial class MusicLibrary : BaseViewModel
    {
        public delegate void EntryFocusHandler();
        public event EntryFocusHandler EntryFocus;

        [ObservableProperty]
        private ObservableCollection<Audio> viewAudio;

        [ObservableProperty]
        protected bool searchIsFocus = false;

        [RelayCommand]
        protected void UnFocus()
        {
            SearchIsFocus = false;
        }

        [RelayCommand]
        protected void SearchFocus()
        {
            SearchIsFocus = true;

            EntryFocus.Invoke();
        }

        [RelayCommand]
        protected void OpenMusic(object obj)
        {
            CollectionView collectionView = obj as CollectionView;
            Audio audio = collectionView.SelectedItem as Audio;
            ObservableCollection<Audio> audios = new ObservableCollection<Audio>(collectionView.ItemsSource as ObservableCollection<Audio>);
            int AudioIndex = 0;

            if (audio.Url == null && !audio.TrackCode.Contains(audio.Title))
            {
                Shell.Current.CurrentPage.DisplayAlert("Ошибка", "Трек не был найден", "Назад");
                
                return;
            }

            foreach (Audio Audio in collectionView.ItemsSource)
            {
                if (audio.Artist == Audio.Artist && audio.Title == Audio.Title)
                    break;

                AudioIndex++;
            }

            PlayerAudios playerAudios = new PlayerAudios()
            {
                PlayingAudio = audio,
                Audios = audios,
                AudioIndex = AudioIndex
            };

            if (audio.Url == null)
            {
                playerAudios.PathToAudio = audio.TrackCode;
            }
            else
            {
                playerAudios.PathToAudio = audio.Url.ToString();
            }

            AudioPlayerService.PlayerAudios = playerAudios;

            if (AudioPlayerService.MusicSet)
            {
                AudioPlayerService.Player.MusicPath = AudioPlayerService.PlayerAudios.PathToAudio;
                AudioPlayerService.Player.PlayerAudios = AudioPlayerService.PlayerAudios;

                if (AudioPlayerService.PlayerAudios.PlayingAudio.Title == audio.Title
                && AudioPlayerService.PlayerAudios.PlayingAudio.Artist == audio.Artist)
                {
                    AudioPlayerService.Player.ImageState = "pause.png";
                }
            }
            Shell.Current.GoToAsync(nameof(AudioPlayerPage));
        }
    }
}
