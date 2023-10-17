﻿using VKMusicApp.Core;
using VKMusicApp.Models;
using VKMusicApp.Services.AudioPlayer.Interfaces;
using VKMusicApp.ViewModels;
using VkNet.Model;

namespace VKMusicApp.Services.AudioPlayer.Implementation
{
    public class AudioPlayerService : ObservableObject, IAudioPlayerService
    {
        private AudioPlayerViewModel player;
        private bool musicSet = false;

        public PlayerAudios PlayerAudios { get; set; }
        public AudioPlayerViewModel Player 
        { 
            get => player;
            set
            {
                player ??= value;
                OnPropertyChanged();
            }
        }
        public bool MusicSet 
        { 
            get => musicSet; 
            set
            {
                musicSet = value;
                OnPropertyChanged();
            }
        }

        public void SetBackAudio()
        {
            try
            {
                PlayerAudios.PlayingAudio = PlayerAudios.Audios[PlayerAudios.AudioIndex - 1];
                PlayerAudios.AudioIndex--;
            }
            catch
            {
                int lastIndex = PlayerAudios.Audios.Count - 1;

                PlayerAudios.PlayingAudio = PlayerAudios.Audios[lastIndex];
                PlayerAudios.AudioIndex = lastIndex;
            }

            SetAudioPath();
        }

        public void SetNewAudio(Audio audio)
        {
            int counter = 0;

            foreach (Audio Audio in PlayerAudios.Audios)
            {
                if (Audio.Artist == audio.Artist && Audio.Title == audio.Title)
                {
                    PlayerAudios.AudioIndex = counter;
                    PlayerAudios.PlayingAudio = Audio;
                    SetAudioPath();

                    return;
                }
                counter++;
            }
        }

        public void SetNextAudio()
        {
            try
            {
                PlayerAudios.PlayingAudio = PlayerAudios.Audios[PlayerAudios.AudioIndex + 1];
                PlayerAudios.AudioIndex++;
            }
            catch
            {
                PlayerAudios.PlayingAudio = PlayerAudios.Audios[0];
                PlayerAudios.AudioIndex = 0;
            }
            SetAudioPath();
        }

        private void SetAudioPath()
        {
            if (PlayerAudios.PlayingAudio.Url != null)
            {
                PlayerAudios.PathToAudio = PlayerAudios.PlayingAudio.Url.ToString();
            }
            else
            {
                PlayerAudios.PathToAudio = PlayerAudios.PlayingAudio.TrackCode;
            }
        }
    }
}
