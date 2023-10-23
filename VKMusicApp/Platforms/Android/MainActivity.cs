using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Controls.Compatibility;
using VKMusicApp.Models;
using VKMusicApp.Platforms.Android;
using VKMusicApp.Services.AudioPlayer.Interfaces;

namespace VKMusicApp;

[Activity(Theme = "@style/Maui.SplashTheme", LaunchMode = LaunchMode.SingleTask, MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.UiMode | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    private NotificationManagerCompat notificationManager;
    private static IMessenger messenger;

    public MainActivity()
    {
        messenger = MauiApplication.Current.Services.GetService<IMessenger>();

        messenger.Register<MessageData>(this, (recipient, message) =>
        {
            SendOnChannel1(message.Title, message.Artist);
        });
    }

    protected override void OnCreate(Bundle savedInstanceState)
    {
        if (!Android.OS.Environment.IsExternalStorageManager)
        {
            Intent intent = new Intent();
            intent.SetAction(Android.Provider.Settings.ActionManageAppAllFilesAccessPermission);
            Android.Net.Uri uri = Android.Net.Uri.FromParts("package", this.PackageName, null);
            intent.SetData(uri);
            StartActivity(intent);
        }

        base.OnCreate(savedInstanceState);

        notificationManager = NotificationManagerCompat.From(this);
    }

    private void SendOnChannel1(string title, string artist)
    {
        Notification notification = MainActivity.GetPlayerNotification(this, title, artist);

        notificationManager.Notify(1, notification);
    }

    public static Notification GetPlayerNotification(Context context, string title, string artist)
    {
        Intent intent = new Intent(context, typeof(MainActivity));
        PendingIntent content = PendingIntent.GetActivity(context, 0, intent, 0);

        Intent backIntent = new Intent(context, typeof(NotificationReceiver));
        backIntent.PutExtra("action", "Back");
        PendingIntent backAction = PendingIntent.GetBroadcast(context, 0, backIntent, PendingIntentFlags.UpdateCurrent);

        Intent playIntent = new Intent(context, typeof(NotificationReceiver));
        playIntent.PutExtra("action", "Play");
        PendingIntent playAction = PendingIntent.GetBroadcast(context, 1, playIntent, PendingIntentFlags.UpdateCurrent);

        Intent nextIntent = new Intent(context, typeof(NotificationReceiver));
        nextIntent.PutExtra("action", "Next");
        PendingIntent nextAction = PendingIntent.GetBroadcast(context, 2, nextIntent, PendingIntentFlags.UpdateCurrent);

        var notification = new NotificationCompat.Builder(context, MainApplication.Channel1Id)
          .SetSmallIcon(Resource.Drawable.blackplay)
          .SetContentTitle(title)
          .SetContentText(artist)
          .SetContentIntent(content)
          .AddAction(Resource.Drawable.blackback, "Back", backAction);

        IAudioPlayerService player = MauiApplication.Current.Services.GetService<IAudioPlayerService>();

        if (player.Player.Player == null ||
            player.Player.Player.CurrentState != CommunityToolkit.Maui.Core.Primitives.MediaElementState.Paused)
        {
            notification.AddAction(Resource.Drawable.blackpause, "Play", playAction);
        }
        else
        {
            notification.AddAction(Resource.Drawable.blackplay, "Play", playAction);
        }
        
        notification.AddAction(Resource.Drawable.blacknext, "Next", nextAction)
          .SetStyle(new AndroidX.Media.App.NotificationCompat.MediaStyle()
                .SetShowActionsInCompactView(0, 1, 2))
          .SetPriority(NotificationCompat.PriorityHigh)
          .SetCategory(NotificationCompat.CategoryService)
          .SetAutoCancel(false)
          .SetSilent(true);

        return notification.Build();
    }
}
