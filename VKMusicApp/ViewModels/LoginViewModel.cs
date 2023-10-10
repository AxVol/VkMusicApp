using System.Windows.Input;
using VKMusicApp.Core;
using VKMusicApp.Pages;
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
        private readonly VkApi vkApi;
        private readonly IFileService fileService;
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

        public LoginViewModel(VkApi VKApi, IFileService service)
        {
            vkApi = VKApi;
            fileService = service;
            
            if (IsLogin().Result)
            {
                Shell.Current.GoToAsync(nameof(AccountMusicPage));
            }

            LoginCommand = new Command(InCommand);
        }

        private async Task<bool> IsLogin()
        {
            try
            {
                string token = await fileService.GetToken();

                await vkApi.AuthorizeAsync(new ApiAuthParams()
                {
                    AccessToken = token,
                });

                return true;
            }
            catch
            {
                return false;
            }
        }

        private async void InCommand()
        {
            PermissionStatus readStatus = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
            PermissionStatus writeStatus = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();

            if (readStatus == PermissionStatus.Granted && writeStatus == PermissionStatus.Granted)
            {
                try
                {
                    ButtonStatus = false;

                    await vkApi.AuthorizeAsync(new ApiAuthParams()
                    {
                        Login = login,
                        Password = password,
                        ApplicationId = 51745723,
                        Settings = VkNet.Enums.Filters.Settings.Audio,
                        TokenExpireTime = 0
                    });

                    await fileService.SetToken(vkApi.Token);
                }
                catch (Exception ex)
                {
                    Exception = ex.Message;

                    ButtonStatus = true;

                    return;
                }

                await Shell.Current.GoToAsync(nameof(AccountMusicPage));
            }
            else
            {
                await Permissions.RequestAsync<Permissions.StorageRead>();
                await Permissions.RequestAsync<Permissions.StorageWrite>();
            }
        }
    }
}
