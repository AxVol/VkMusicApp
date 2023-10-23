using Android.Content;
using AndroidX.Core.App;
using Microsoft.Maui.Platform;
using VKMusicApp.Services.AudioPlayer.Interfaces;

namespace VKMusicApp.Platforms.Android
{
    [BroadcastReceiver(Enabled = true, Exported = false)]
    public class NotificationReceiver : BroadcastReceiver
    {
        private IAudioPlayerService player;

        public override void OnReceive(Context context, Intent intent)
        {
            string message = intent.GetStringExtra("action");
            player ??= MauiApplication.Current.Services.GetService<IAudioPlayerService>();

            switch (message)
            {
                case "Back":
                    player.Player.BackCommand.Execute(null);

                    break;
                case "Play":
                    player.Player.PlayCommand.Execute(null);
                    
                    break;
                case "Next":
                    player.Player.NextCommand.Execute(null);

                    break;
            }
        }
    }
}
