using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VKMusicApp.Services.AudioPlayer.Interfaces;
using VKMusicApp.Services.Interfaces;
using VkNet.Model;

namespace VKMusicApp.Core
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        public Color navigationBackground = Color.FromArgb("#2C2E44");

        [ObservableProperty]
        protected IAudioPlayerService audioPlayerService;

        [ObservableProperty]
        protected IFileService fileService;

        [RelayCommand]
        private static void GoTo(object obj)
        {
            string page = obj as string;

            Shell.Current.GoToAsync(page);
        }

        // Всплывашка которая срабатывает по нажатию кнопки в правой части списка музыки
        [RelayCommand]
        protected async Task ShowPopUp(object obj)
        {
            string action;

            Audio audio = obj as Audio;

            if (await FileService.MusicInStorage(audio))
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
                    await FileService.SaveMusic(audio);
                    break;
                case "Удалить":
                    await FileService.DeleteMusic(audio);
                    break;
            }
        }
    }
}