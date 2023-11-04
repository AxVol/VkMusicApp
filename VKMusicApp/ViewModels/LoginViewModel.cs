using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VKMusicApp.Core;
using VKMusicApp.Models;
using VKMusicApp.Pages;
using VKMusicApp.Services.Interfaces;
using VkNet;
using VkNet.Model;

namespace VKMusicApp.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        private readonly VkApi vkApi;

        [ObservableProperty]
        private string login;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private bool buttonStatus = true;

        [ObservableProperty]
        private string exception;

        public LoginViewModel(VkApi VKApi, IFileService service)
        {
            vkApi = VKApi;
            FileService = service;
        }

        [RelayCommand]
        private async Task LogIn()
        {
            PermissionStatus readStatus = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
            PermissionStatus writeStatus = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();

            if (readStatus == PermissionStatus.Granted && writeStatus == PermissionStatus.Granted)
            {
                try
                {
                    ButtonStatus = false;

                    if (!(await LoginWithoutTFAsync(Login, Password)))
                    {
                        string tf = await Shell.Current.CurrentPage.DisplayPromptAsync("Двухфакторная авторизация", "Код", "Войти", "Отмена");

                        if (tf == null || tf == string.Empty)
                            return;

                        if (!(await LoginWithTFAsync(Login, Password, tf)))
                            return;                      
                    }

                    await FileService.SetConfig(Login, Password);
                }
                catch (Exception ex)
                {
                    Exception = ex.Message;
                    ButtonStatus = true;

                    return;
                }

                await Shell.Current.GoToAsync(nameof(AccountMusicPage));

                ButtonStatus = true;
                Login = string.Empty;
                Password = string.Empty;
            }
            else
            {
                await Permissions.RequestAsync<Permissions.StorageRead>();
                await Permissions.RequestAsync<Permissions.StorageWrite>();
            }
        }

        public async Task<bool> IsLogin()
        {
            try
            {
                VkPlayerConfig config = await FileService.GetConfig();
                Login = config.Login;
                Password = config.Password;

                if (!(LoginWithoutTF(Login, Password)))
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("Bad"))
                {
                    return await IsLogin(); 
                }

                return false;
            }
        }

        public static bool HasEthernet()
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;

            if (accessType == NetworkAccess.Internet)
            {
                return true;
            }

            return false;
        }

        private bool LoginWithoutTF(string login, string password)
        {
            try
            {
                vkApi.Authorize(new ApiAuthParams
                {
                    Login = login,
                    Password = password,
                    ApplicationId = 51745723,
                    Settings = VkNet.Enums.Filters.Settings.Audio,
                });

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> LoginWithTFAsync(string login, string password, string tf)
        {
            try
            {
                await vkApi.AuthorizeAsync(new ApiAuthParams
                {
                    Login = login,
                    Password = password,
                    ApplicationId = 51745723,
                    Settings = VkNet.Enums.Filters.Settings.Audio,
                    TwoFactorAuthorization = () => tf
                });

                return true;
            }
            catch (Exception e)
            {
                Exception = e.Message;

                return false;
            }
        }

        private async Task<bool> LoginWithoutTFAsync(string login, string password)
        {
            try
            {
                await vkApi.AuthorizeAsync(new ApiAuthParams
                {
                    Login = login,
                    Password = password,
                    ApplicationId = 51745723,
                    Settings = VkNet.Enums.Filters.Settings.Audio,
                });

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
