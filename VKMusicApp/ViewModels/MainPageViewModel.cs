using System;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using VKMusicApp.Core;
using VKMusicApp.Pages;

namespace VKMusicApp.ViewModels
{
    public class MainPageViewModel : ObservableObject
    {
        private int count;

        public ICommand Test { get; set; }
        public ICommand Next { get; set; }
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
            Next = new Command(() =>
            {
                INavigation navigation = App.Current.MainPage.Navigation;
                navigation.PushAsync(new NewPage1());
            });
        }
    }
}
