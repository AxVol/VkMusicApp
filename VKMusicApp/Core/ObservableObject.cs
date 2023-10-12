using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using VKMusicApp.Services.AudioPlayer.Interfaces;
using VKMusicApp.Services.Interfaces;
using VkNet.Model;

namespace VKMusicApp.Core
{
    public class ObservableObject : INotifyPropertyChanged
    {
        protected IAudioPlayerService audioPlayerService;
        protected IFileService fileService;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand GoTo { get; set; } = new Command(GoToPage);
        public ICommand ShowPopUpCommand { get; set; }

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
            Audio audio = obj as Audio;

            if (await fileService.MusicInStorage(audio))
            {
                string action = await Shell.Current.CurrentPage.DisplayActionSheet("Действие", "Назад", null, "Удалить");

                switch (action)
                {
                    case "Удалить":
                        await fileService.DeleteMusic(audio);
                        break;
                }
            }
            else
            {
                string action = await Shell.Current.CurrentPage.DisplayActionSheet("Действие", "Назад", null, "Скачать");

                switch (action)
                {
                    case "Скачать":
                        await fileService.SaveMusic(audio);
                        break;
                }
            }
        }
    }
}