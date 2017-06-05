using Foundation;
using UIKit;
using Firebase.Analytics;
using Google.SignIn;
using System;
using Firebase.CloudMessaging;
using Firebase.InstanceID;
using Firebase.Database;
using UserNotifications;

namespace FirebaseXamarin.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
	[Register("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate, IUNUserNotificationCenterDelegate
	{
		public event EventHandler<UserInfoEventArgs> NotificationReceived;

		public static AppDelegate applicationDelegate()
		{
			return UIApplication.SharedApplication.Delegate as AppDelegate;
		}

		// class-level declarations
		public override UIWindow Window
		{
			get;
			set;
		}

		public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
		{
			// Override point for customization after application launch.
			// If not required for your application you can safely delete this method

			// Code to start the Xamarin Test Cloud Agent
#if ENABLE_TEST_CLOUD
			Xamarin.Calabash.Start();
#endif

			App.Configure();

			// You can get the GoogleService-Info.plist file at https://developers.google.com/mobile/add
			var googleServiceDictionary = NSDictionary.FromFile("GoogleService-Info.plist");
			SignIn.SharedInstance.ClientID = googleServiceDictionary["CLIENT_ID"].ToString();

			Database.DefaultInstance.PersistenceEnabled = true;

			DBManager.sharedManager.createTables();


			//Configuring the APNS for Notifications
			configureAPNS();

			return true;
		}

		public override void OnResignActivation(UIApplication application)
		{
			// Invoked when the application is about to move from active to inactive state.
			// This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
			// or when the user quits the application and it begins the transition to the background state.
			// Games should use this method to pause the game.
		}

		public override void DidEnterBackground(UIApplication application)
		{
			// Use this method to release shared resources, save user data, invalidate timers and store the application state.
			// If your application supports background exection this method is called instead of WillTerminate when the user quits.
			//  Messaging.SharedInstance.Disconnect();
			// Console.WriteLine("Disconnected from FCM");
		}

		public override void WillEnterForeground(UIApplication application)
		{
			// Called as part of the transiton from background to active state.
			// Here you can undo many of the changes made on entering the background.
		}

		public override void OnActivated(UIApplication application)
		{
			// Restart any tasks that were paused (or not yet started) while the application was inactive. 
			// If the application was previously in the background, optionally refresh the user interface.
		}

		public override void WillTerminate(UIApplication application)
		{
			// Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
		}

		// For iOS 9 or newer
		public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
		{
			var openUrlOptions = new UIApplicationOpenUrlOptions(options);
			return SignIn.SharedInstance.HandleUrl(url, openUrlOptions.SourceApplication, openUrlOptions.Annotation);
		}

		// For iOS 8 and older
		public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
		{
			return SignIn.SharedInstance.HandleUrl(url, sourceApplication, annotation);
		}

		public void moveToUsersScreen()
		{
			var tabsScreen = UIStoryboard.FromName("Main", NSBundle.MainBundle).InstantiateViewController("ChatTabBarController");
			Window.RootViewController = new UINavigationController(tabsScreen);
			Window.MakeKeyAndVisible();
		}

		public void moveToLoginScreen()
		{
			var loginScreen = UIStoryboard.FromName("Main", NSBundle.MainBundle).InstantiateViewController("LoginViewController");
			Window.RootViewController = loginScreen;
			Window.MakeKeyAndVisible();
		}

		public void chooseWhereToGo()
		{
			if (DBManager.sharedManager.getLoggedInUserInfo() != null)
			{
				moveToUsersScreen();
			}
			else
			{
				moveToLoginScreen();
			}
		}

		private void configureAPNS()
		{
			// Monitor token generation
			InstanceId.Notifications.ObserveTokenRefresh(TokenRefreshNotification);

			// Register your app for remote notifications.
			if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
			{
				// iOS 10 or later
				var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
				UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) =>
				{
					Console.WriteLine(granted);
				});

				// For iOS 10 display notification (sent via APNS)
				UNUserNotificationCenter.Current.Delegate = this;

				// For iOS 10 data message (sent via FCM)
				//Messaging.SharedInstance.RemoteMessageDelegate = this;
			}
			else
			{
				// iOS 9 or before
				var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
				var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
				UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
			}

			UIApplication.SharedApplication.RegisterForRemoteNotifications();
		}

		public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
		{

		}

		// To receive notifications in foregroung on iOS 9 and below.
		// To receive notifications in background in any iOS version
		public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
		{
			// If you are receiving a notification message while your app is in the background,
			// this callback will not be fired 'till the user taps on the notification launching the application.

			// If you disable method swizzling, you'll need to call this method. 
			// This lets FCM track message delivery and analytics, which is performed
			// automatically with method swizzling enabled.
			//Messaging.GetInstance ().AppDidReceiveMessage (userInfo);

			if (NotificationReceived == null)
				return;

			var e = new UserInfoEventArgs { UserInfo = userInfo };
			NotificationReceived(this, e);
		}

		// You'll need this method if you set "FirebaseAppDelegateProxyEnabled": NO in GoogleService-Info.plist
		public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
		{
			InstanceId.SharedInstance.SetApnsToken(deviceToken, ApnsTokenType.Sandbox);
		}

		public static void ConnectToFCM(UIViewController fromViewController)
		{
			Messaging.SharedInstance.Connect(error =>
			{
				if (error != null)
				{
					//ShowMessage("Unable to connect to FCM", error.LocalizedDescription, fromViewController);
					Console.WriteLine("Error in connecting to FCM" + error);
				}
				else
				{
					//ShowMessage("Success!", "Connected to FCM", fromViewController);
					var token = InstanceId.SharedInstance.Token;
					Console.WriteLine($"Token: {token}");
					if (!String.IsNullOrEmpty(token))
					{
						NSUserDefaults.StandardUserDefaults.SetString(token, FirebaseConstants.FB_TOKEN);
						NSUserDefaults.StandardUserDefaults.Synchronize();
					}
				}
			});
		}

		void TokenRefreshNotification(object sender, NSNotificationEventArgs e)
		{
			// This method will be fired everytime a new token is generated, including the first
			// time. So if you need to retrieve the token as soon as it is available this is where that
			// should be done.
			//var refreshedToken = InstanceId.SharedInstance.Token;


			Console.WriteLine($"Token Refreshed: {InstanceId.SharedInstance.Token}");
			ConnectToFCM(Window.RootViewController);
			var token = InstanceId.SharedInstance.Token;
			if (!String.IsNullOrEmpty(token))
			{
				NSUserDefaults.StandardUserDefaults.SetString(token, FirebaseConstants.FB_TOKEN);
			}
			// TODO: If necessary send token to application server.
		}

		#region Workaround for handling notifications in background for iOS 10
		[Export("userNotificationCenter:didReceiveNotificationResponse:withCompletionHandler:")]
		public void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
		{
			if (NotificationReceived == null)
				return;

			var e = new UserInfoEventArgs { UserInfo = response.Notification.Request.Content.UserInfo };
			NotificationReceived(this, e);
		}
		#endregion

		public class UserInfoEventArgs : EventArgs
		{
			public NSDictionary UserInfo { get; set; }
		}


	}
}

