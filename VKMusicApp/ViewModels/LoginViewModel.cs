using System.Windows.Input;
using VKMusicApp.Core;
using VKMusicApp.Pages;
using VkNet;
using VkNet.Model;

namespace VKMusicApp.ViewModels
{
    public class LoginViewModel : ObservableObject
    {
        private string login;
        private string password;
        private bool buttonStatus = true;
        private VkApi vkApi;
        private string exception;

        public ICommand LoginCommand { get; set; }
        public bool ButtonStatus 
        { 
            get => buttonStatus; 
            set
            {
                buttonStatus = value;
                OnPropertyChanged();
            }
        }
        public string Login
        {
            get => login;
            set 
            {
                login = value;
                OnPropertyChanged();
            }
        }
        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged();
            }
        }
        public string Exception
        {
            get => exception;
            set
            {
                exception = value;
                OnPropertyChanged();
            }
        }

        public LoginViewModel(VkApi VKApi)
        {
            vkApi = VKApi;
            LoginCommand = new Command(InCommand);
        }

        private void InCommand()
        {
            try
            {
                vkApi.Authorize(new ApiAuthParams()
                {
                    Login = login,
                    Password = password,
                    ApplicationId = 51745723,
                    Settings = VkNet.Enums.Filters.Settings.Audio
                });

                Shell.Current.GoToAsync(nameof(MusicLibraryPage));
            }
            catch (Exception ex)
            {
                Exception = ex.Message;
            }
        }
    }
}
