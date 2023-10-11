using System.Windows.Input;
using CommunityToolkit.Maui.Storage;
using VKMusicApp.Core;
using VKMusicApp.Pages;
using VKMusicApp.Services.Interfaces;

namespace VKMusicApp.ViewModels
{
    public class SettingsViewModel : ObservableObject
    {
        private readonly IFileService fileService;
        private string pathToSave;

        public string PathToSave 
        { 
            get => pathToSave; 
            set
            {
                pathToSave = value;
                OnPropertyChanged();
            }
        }

        public ICommand BackCommand { get; set; }
        public ICommand SetPathToSaveCommand { get; set; }
        public ICommand ExitCommand { get; set; }

        public SettingsViewModel(IFileService service) 
        {
            fileService = service;

            PathToSave = fileService.PathToSave;

            BackCommand = new Command(Back);
            SetPathToSaveCommand = new Command(SetPathToSave);
            ExitCommand = new Command(Exit);
        }

        private async void Back()
        {
            await Shell.Current.Navigation.PopAsync();
        }

        private async void SetPathToSave()
        {
            try
            {
                FolderPickerResult folder = await FolderPicker.PickAsync(default);

                PathToSave = folder.Folder.Path;
                await fileService.SetPathToSave(pathToSave);
            }
            catch
            {

            }
        }

        private async void Exit()
        {
            await fileService.DeleteLoginAndPass();

            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}
