﻿using System.Text.RegularExpressions;
using VKMusicApp.Models;
using VKMusicApp.Services.AudioPlayer.Interfaces;
using VkNet.Model;

namespace VKMusicApp.Services.AudioPlayer.Implementation
{
    public class AudioPlayerService : IAudioPlayerService
    {
        public PlayerAudios PlayerAudios { get; set; }
        public bool IsOnline { get; set; }

        public void SetBackAudio()
        {
            try
            {
                PlayerAudios.PlayingAudio = PlayerAudios.Audios[PlayerAudios.AudioIndex - 1];
                PlayerAudios.AudioIndex--;
                PlayerAudios.PathToAudio = UrlConverter(PlayerAudios.PlayingAudio.Url);
            }
            catch
            {
                int lastIndex = PlayerAudios.Audios.Count - 1;

                PlayerAudios.PlayingAudio = PlayerAudios.Audios[lastIndex];
                PlayerAudios.AudioIndex = lastIndex;
                PlayerAudios.PathToAudio = UrlConverter(PlayerAudios.PlayingAudio.Url);
            }
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
                    PlayerAudios.PathToAudio = UrlConverter(Audio.Url);

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
                PlayerAudios.PathToAudio = UrlConverter(PlayerAudios.PlayingAudio.Url);
            }
            catch
            {
                PlayerAudios.PlayingAudio = PlayerAudios.Audios[0];
                PlayerAudios.AudioIndex = 0;
                PlayerAudios.PathToAudio = UrlConverter(PlayerAudios.PlayingAudio.Url);
            }
        }

        public string UrlConverter(Uri Url)
        {
            string url = Regex.Replace(
                Url.ToString(),
                @"/[a-zA-Z\d]{6,}(/.*?[a-zA-Z\d]+?)/index.m3u8()",
                @"$1$2.mp3");

            return url;
        }
    }
}
