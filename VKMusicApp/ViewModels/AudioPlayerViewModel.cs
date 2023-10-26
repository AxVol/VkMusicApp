using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using VKMusicApp.Core;
using VKMusicApp.Models;
using VKMusicApp.Services.AudioPlayer.Interfaces;
using VKMusicApp.Services.Interfaces;
using VkNet.Model;

namespace VKMusicApp.ViewModels
{
    public partial class AudioPlayerViewModel : BaseViewModel
    {
        private readonly IMessenger messenger;

        [ObservableProperty]
        private string imageState = "pause.png";

        [ObservableProperty]
        private string loopImage = "loop.png";

        [ObservableProperty]
        private MediaSource musicPath;

        private MediaElement player;
        public MediaElement Player 
        {   get => player; 
            set
            {
                player = value;
                Player.MediaEnded += NextMedia;
                OnPropertyChanged(nameof(Player));
            }
        }

        private PlayerAudios playerAudios;
        public PlayerAudios PlayerAudios
        {
            get => playerAudios;
            set
            {
                playerAudios = value;
                if (!playerAudios.IsShuffle)
                {
                    if (playerAudios.PathToAudio.StartsWith("storage/emulated"))
                    {
                        MusicPath = MediaSource.FromFile(playerAudios.PathToAudio);
                    }
                    else
                    {
                        MusicPath = MediaSource.FromUri(playerAudios.PathToAudio);
                    }
                }
                OnPropertyChanged(nameof(PlayerAudios));
            }
        }

        public AudioPlayerViewModel(IAudioPlayerService service, IFileService file, IMessenger Messenger)
        {
            AudioPlayerService = service;
            PlayerAudios = audioPlayerService.PlayerAudios;
            FileService = file;
            messenger = Messenger;

            AudioPlayerService.MusicSet = true;
            AudioPlayerService.Player = this;

            messenger.Send(new MessageData(1, PlayerAudios.PlayingAudio.Title, PlayerAudios.PlayingAudio.Artist));
        }

        [RelayCommand]
        private void Play()
        {
            if (Player.CurrentState == CommunityToolkit.Maui.Core.Primitives.MediaElementState.Playing)
            {
                Player.Pause();
                ImageState = "play.png";
            }
            else
            {
                Player.Play();
                ImageState = "pause.png";
            }

            messenger.Send(new MessageData(1, PlayerAudios.PlayingAudio.Title, PlayerAudios.PlayingAudio.Artist));
        }

        [RelayCommand]
        private void Next()
        {
            Player.Stop();

            if (Player.CurrentState == CommunityToolkit.Maui.Core.Primitives.MediaElementState.Stopped)
                ImageState = "pause.png";

            AudioPlayerService.SetNextAudio();
            PlayerAudios = AudioPlayerService.PlayerAudios;

            Player.Play();
            messenger.Send(new MessageData(1, PlayerAudios.PlayingAudio.Title, PlayerAudios.PlayingAudio.Artist));
        }

        [RelayCommand]
        private void Back()
        {
            Player.Stop();

            if (Player.CurrentState == CommunityToolkit.Maui.Core.Primitives.MediaElementState.Stopped)
                ImageState = "pause.png";

            AudioPlayerService.SetBackAudio();
            PlayerAudios = AudioPlayerService.PlayerAudios;

            Player.Play();
            messenger.Send(new MessageData(1, PlayerAudios.PlayingAudio.Title, PlayerAudios.PlayingAudio.Artist));
        }

        [RelayCommand]
        private void Shuffle()
        {
            Random random = new Random();
            int listLength = AudioPlayerService.PlayerAudios.Audios.Count;

            while (listLength > 1)
            {
                listLength--;

                int randNumber = random.Next(listLength + 1);
                (AudioPlayerService.PlayerAudios.Audios[randNumber], AudioPlayerService.PlayerAudios.Audios[listLength]) = 
                    (AudioPlayerService.PlayerAudios.Audios[listLength], AudioPlayerService.PlayerAudios.Audios[randNumber]);
            }

            AudioPlayerService.PlayerAudios.Audios.Remove(AudioPlayerService.PlayerAudios.PlayingAudio);
            AudioPlayerService.PlayerAudios.Audios.Insert(0, AudioPlayerService.PlayerAudios.PlayingAudio);
            AudioPlayerService.PlayerAudios.AudioIndex = 0;

            AudioPlayerService.PlayerAudios.IsShuffle = true;
            PlayerAudios = AudioPlayerService.PlayerAudios;
            AudioPlayerService.PlayerAudios.IsShuffle = false;
        }

        [RelayCommand]
        private void Loop()
        {
            if (!Player.ShouldLoopPlayback)
            {
                LoopImage = "loopactive.png";
                Player.ShouldLoopPlayback = true;

                return;
            }

            Player.ShouldLoopPlayback = false;
            LoopImage = "loop.png";
        }

        [RelayCommand]
        private void Rewind(object obj) 
        {
            double position = (double)obj;

            TimeSpan time = TimeSpan.FromSeconds(position);

            Player.SeekTo(time);
        }

        [RelayCommand]
        private void ChangeMusic(object obj)
        {
            CollectionView collectionView = obj as CollectionView;
            Audio audio = collectionView.SelectedItem as Audio;

            AudioPlayerService.SetNewAudio(audio);
            PlayerAudios = AudioPlayerService.PlayerAudios;

            ImageState = "pause.png";

            messenger.Send(new MessageData(1, PlayerAudios.PlayingAudio.Title, PlayerAudios.PlayingAudio.Artist));
        }

        private void NextMedia(object sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Next();
            });
        }
    }
}
