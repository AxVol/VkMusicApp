using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VKMusicApp.Core;
using VKMusicApp.Pages;
using VKMusicApp.Services.Interfaces;

namespace VKMusicApp.ViewModels
{
    public partial class SettingsViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string pathToSave;

        public SettingsViewModel(IFileService service) 
        {
            FileService = service;

            PathToSave = FileService.PathToSave;
        }

        [RelayCommand]
        private async Task Back()
        {
            await Shell.Current.Navigation.PopAsync();
        }

        [RelayCommand]
        private async Task SetPathToSave()
        {
            try
            {
                FolderPickerResult folder = await FolderPicker.PickAsync(default);

                PathToSave = folder.Folder.Path;
                await FileService.SetPathToSave(PathToSave);
            }
            catch
            {

            }
        }

        [RelayCommand]
        private async Task Exit()
        {
            await FileService.DeleteLoginAndPass();

            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}
