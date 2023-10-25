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

                    await vkApi.AuthorizeAsync(new ApiAuthParams
                    {
                        Login = Login,
                        Password = Password,
                        ApplicationId = 51745723,
                        Settings = VkNet.Enums.Filters.Settings.Audio
                    });

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

                vkApi.Authorize(new ApiAuthParams
                {
                    Login = config.Login,
                   Password = config.Password,
                    ApplicationId = 51745723,
                    Settings = VkNet.Enums.Filters.Settings.Audio
                });

                return true;
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("Bad"))
                {
                    return IsLogin().Result; 
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
    }
}
