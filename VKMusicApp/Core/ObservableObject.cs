﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using VKMusicApp.Pages;
using VKMusicApp.Services.AudioPlayer.Interfaces;
using VKMusicApp.Services.Interfaces;
using VkNet.Model;

namespace VKMusicApp.Core
{
    public class ObservableObject : INotifyPropertyChanged
    {
        protected Color navigationBackground = Color.FromArgb("#2C2E44");
        protected IAudioPlayerService audioPlayerService;
        protected IFileService fileService;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand GoTo { get; set; } = new Command(GoToPage);
        public ICommand ShowPopUpCommand { get; set; }

        public Color NavigationBackground
        {
            get => navigationBackground;
            set
            {
                navigationBackground = value;
                OnPropertyChanged();
            }
        }

        public IAudioPlayerService AudioPlayerService
        {
            get => audioPlayerService;
            set
            {
                audioPlayerService = value;
                OnPropertyChanged();
            }
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static void GoToPage(object obj)
        {
            string page = obj as string;

            Shell.Current.GoToAsync(page);
        }

        protected async void ShowPopUp(object obj)
        {
            string action;

            Audio audio = obj as Audio;

            if (await fileService.MusicInStorage(audio))
            {
                action = await Shell.Current.CurrentPage.DisplayActionSheet("Действие", "Назад", null, "Удалить");
            }
            else
            {
                action = await Shell.Current.CurrentPage.DisplayActionSheet("Действие", "Назад", null, "Скачать");
            }

            switch (action)
            {
                case "Скачать":
                    await fileService.SaveMusic(audio);
                    break;
                case "Удалить":
                    await fileService.DeleteMusic(audio);
                    break;
            }
        }
    }
}