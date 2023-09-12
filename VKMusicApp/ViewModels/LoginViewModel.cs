using System.Windows.Input;
using VKMusicApp.Core;
using VKMusicApp.Services.Interfaces;
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
        private string test;

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
        public string Test
        {
            get => test;
            set
            {
                test = value;
                OnPropertyChanged();
            }
        }

        public LoginViewModel(IVkApiProvider VKApi)
        {
            vkApi = VKApi.VkApi;
            LoginCommand = new Command(InCommand);
        }

        private void InCommand()
        {
            vkApi.Authorize(new ApiAuthParams()
            {
                Login = login,
                Password = password,
                ApplicationId = 51745723,
                Settings = VkNet.Enums.Filters.Settings.Audio
            });

            Test = vkApi.Token;
        }
    }
}
