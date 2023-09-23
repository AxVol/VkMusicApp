using CommunityToolkit.Maui.Views;
using System.Windows.Input;
using VKMusicApp.Core;

namespace VKMusicApp.ViewModels
{
    public class AudioPlayerViewModel : ObservableObject
    {
        private MediaElement player;
        private string imageState = "play.png";
        private MediaSource musicPath = MediaSource.FromResource("nf_change.mp3");

        public string ImageState 
        { 
            get => imageState; 
            set
            {
                imageState = value;
                OnPropertyChanged();
            }
        }

        public MediaSource MusicPath
        {
            get => musicPath;
            set
            {
                musicPath = value;
                OnPropertyChanged();
            }
        }

        public ICommand PlayCommand { get; set; }
        public ICommand NextCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand ShuffleCommand { get; set; }
        public ICommand LoopCommand { get; set; }
        public ICommand RewindCommand { get; set; }

        public AudioPlayerViewModel()
        {
            PlayCommand = new Command(Play);
            NextCommand = new Command(Next);
            BackCommand = new Command(Back);
            ShuffleCommand = new Command(Shuffle);
            LoopCommand = new Command(Loop);
            RewindCommand = new Command(Rewind);
        }

        private void Play(object obj)
        {
            player = obj as MediaElement;

            if (player.CurrentState == CommunityToolkit.Maui.Core.Primitives.MediaElementState.Playing)
            {
                player.Pause();
                ImageState = "play.png";
            }
            else
            {
                player.Play();
                ImageState = "pause.png";
            }
        }

        private void Next(object obj)
        {
            
        }

        private void Back(object obj)
        {
            
        }

        private void Shuffle(object obj)
        {
            
        }

        private void Loop(object obj)
        {
            
        }

        private void Rewind(object obj) 
        {
            ValueChangedEventArgs position = obj as ValueChangedEventArgs;

            TimeSpan time = TimeSpan.FromSeconds(position.NewValue);

            player.SeekTo(time);
        }
    }
}
