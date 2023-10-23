using Android.App;
using Android.OS;
using Android.Runtime;

namespace VKMusicApp;

[Application]
public class MainApplication : MauiApplication
{
    public const string Channel1Id = "channel1";

    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
		: base(handle, ownership)
	{
	}

    public override void OnCreate()
    {
        base.OnCreate();

        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        {
            var channel1 = new NotificationChannel(Channel1Id, "Channel 1", NotificationImportance.High);
            channel1.Description = "Channel1";

            if (GetSystemService(NotificationService) is NotificationManager manager)
            {
                manager.CreateNotificationChannel(channel1);
            }
        }
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
