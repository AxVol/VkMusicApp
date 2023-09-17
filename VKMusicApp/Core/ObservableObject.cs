using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace VKMusicApp.Core
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public ICommand GoTo { get; set; } = new Command(GoToPage);

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static async void GoToPage(object obj)
        {
            string page = obj as string;

            await Shell.Current.GoToAsync(page);
        }
    }
}