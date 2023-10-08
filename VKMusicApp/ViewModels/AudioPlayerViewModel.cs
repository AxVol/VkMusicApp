﻿using CommunityToolkit.Maui.Views;
using System.Windows.Input;
using VKMusicApp.Core;
using VKMusicApp.Models;
using VKMusicApp.Services.AudioPlayer.Interfaces;
using VkNet.Model;

namespace VKMusicApp.ViewModels
{
    public class AudioPlayerViewModel : ObservableObject
    {
        private string imageState = "pause.png";
        private string loopimage = "loop.png";
        private MediaSource musicPath = MediaSource.FromResource("nf_change.mp3");
        private MediaElement player;
        private PlayerAudios playerAudios;

        public MediaElement Player 
        {   get => player; 
            set
            {
                player = value;
                Player.MediaEnded += NextMedia;

                OnPropertyChanged();
            }
        }

        public string LoopImage
        {
            get => loopimage;
            set
            {
                loopimage = value;
                OnPropertyChanged();
            }
        }

        public PlayerAudios PlayerAudios
        {
            get => playerAudios;
            set
            {
                playerAudios = value;
                if (!playerAudios.IsShuffle)
                {
                    MusicPath = MediaSource.FromUri(playerAudios.PathToAudio);
                }

                OnPropertyChanged();
            }
        }

        public string ImageState 
        { 
            get => imageState; 
            set
            {
                imageState = value;
                OnPropertyChanged();
            }
        }

        public MediaSource MusicPath
        {
            get => musicPath;
            set
            {
                musicPath = value;
                OnPropertyChanged();
            }
        }

        public ICommand PlayCommand { get; set; }
        public ICommand NextCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand ShuffleCommand { get; set; }
        public ICommand LoopCommand { get; set; }
        public ICommand RewindCommand { get; set; }
        public ICommand ChangeMusicCommand { get; set; }

        public AudioPlayerViewModel(IAudioPlayerService service)
        {
            audioPlayerService = service;
            PlayerAudios = service.PlayerAudios;

            audioPlayerService.MusicSet = true;

            PlayCommand = new Command(Play);
            NextCommand = new Command(Next);
            BackCommand = new Command(Back);
            ShuffleCommand = new Command(Shuffle);
            LoopCommand = new Command(Loop);
            RewindCommand = new Command(Rewind);
            ChangeMusicCommand = new Command(ChangeMusic);

            audioPlayerService.Player = this;
        }

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
        }

        private void Next()
        {
            Player.Stop();

            audioPlayerService.SetNextAudio();
            PlayerAudios = audioPlayerService.PlayerAudios;

            if (Player.CurrentState == CommunityToolkit.Maui.Core.Primitives.MediaElementState.Stopped)
                ImageState = "pause.png";

            Player.Play();
        }

        private void Back()
        {
            Player.Stop();

            audioPlayerService.SetBackAudio();
            PlayerAudios = audioPlayerService.PlayerAudios;

            if (Player.CurrentState == CommunityToolkit.Maui.Core.Primitives.MediaElementState.Stopped)
                ImageState = "pause.png";

            Player.Play();
        }

        private void Shuffle()
        {
            Random random = new Random();
            int listLength = audioPlayerService.PlayerAudios.Audios.Count;

            while (listLength > 1)
            {
                listLength--;

                int randNumber = random.Next(listLength + 1);
                Audio audio = audioPlayerService.PlayerAudios.Audios[randNumber];
                audioPlayerService.PlayerAudios.Audios[randNumber] = audioPlayerService.PlayerAudios.Audios[listLength];
                audioPlayerService.PlayerAudios.Audios[listLength] = audio;
            }

            audioPlayerService.PlayerAudios.Audios.Remove(audioPlayerService.PlayerAudios.PlayingAudio);
            audioPlayerService.PlayerAudios.Audios.Insert(0, audioPlayerService.PlayerAudios.PlayingAudio);
            audioPlayerService.PlayerAudios.AudioIndex = 0;

            audioPlayerService.PlayerAudios.IsShuffle = true;
            PlayerAudios = audioPlayerService.PlayerAudios;
            audioPlayerService.PlayerAudios.IsShuffle = false;
        }

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

        private void Rewind(object obj) 
        {
            double position = (double)obj;

            TimeSpan time = TimeSpan.FromSeconds(position);

            Player.SeekTo(time);
        }

        private void NextMedia(object sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Next();
            });
        }

        private void ChangeMusic(object obj)
        {
            CollectionView collectionView = obj as CollectionView;
            Audio audio = collectionView.SelectedItem as Audio;

            audioPlayerService.SetNewAudio(audio);
            PlayerAudios = audioPlayerService.PlayerAudios;
        }
    }
}
