using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using VKMusicApp.Services.AudioPlayer.Interfaces;

namespace VKMusicApp.Core
{
    public class ObservableObject : INotifyPropertyChanged
    {
        protected IAudioPlayerService audioPlayerService;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand GoTo { get; set; } = new Command(GoToPage);

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
    }
}