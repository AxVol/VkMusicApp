﻿using System.Collections.ObjectModel;
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

            foreach (Audio Audio in collectionView.ItemsSource)
            {
                audios.Add(Audio);
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

                    Shell.Current.GoToAsync(nameof(AudioPlayerPage));

                    return;
                }
            }

            Shell.Current.GoToAsync(nameof(AudioPlayerPage));
        }
    }
}
