using System;
using System.Windows.Input;
using VKMusicApp.Core;

namespace VKMusicApp.ViewModels
{
    public class MainPageViewModel : ObservableObject
    {
        private int count;

        public ICommand Test { get; set; }
        public int Count
        {
            get => count;
            set
            {
                count = value;
                OnPropertyChanged();
            }
        }

        public MainPageViewModel()
        {
            Test = new Command(() =>
            {
                Count++;
            });
        }
    }
}
