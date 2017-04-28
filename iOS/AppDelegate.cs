﻿using Foundation;
using UIKit;
using Firebase.Analytics;
using Google.SignIn;
using System;
using Firebase.CloudMessaging;
using Firebase.InstanceID;
using Firebase.Database;

namespace FirebaseXamarin.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {

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

            //Configuring the APNS for Notifications
            configureAPNS();

            // You can get the GoogleService-Info.plist file at https://developers.google.com/mobile/add
            var googleServiceDictionary = NSDictionary.FromFile("GoogleService-Info.plist");
            SignIn.SharedInstance.ClientID = googleServiceDictionary["CLIENT_ID"].ToString();

            //Firebase configuration
            App.Configure();
            Database.DefaultInstance.PersistenceEnabled = true;

            DBManager.sharedManager.createTables();

            chooseWhereToGo();
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
            Messaging.SharedInstance.Disconnect();
            Console.WriteLine("Disconnected from FCM");
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
            var usersScreen = UIStoryboard.FromName("Main", NSBundle.MainBundle).InstantiateViewController("UsersViewController");
            Window.RootViewController = new UINavigationController(usersScreen);
            Window.MakeKeyAndVisible();
        }

        public void moveToLoginScreen()
        {
            var loginScreen = UIStoryboard.FromName("Main", NSBundle.MainBundle).InstantiateViewController("LoginViewController");
            Window.RootViewController = loginScreen;
            Window.MakeKeyAndVisible();
        }

        private void chooseWhereToGo()
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
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
                                   UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                                   new NSSet());

                UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }
            else
            {
                UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
                UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
            }

            Messaging.SharedInstance.Connect(error =>
            {
                if (error != null)
                {
                    // Handle if something went wrong while connecting
                }
                else
                {
                    // Let the user know that connection was successful
                    var token = InstanceId.SharedInstance.Token;
                    Console.WriteLine("FCM device token is - " + token);
					// Monitor token generation
					NSUserDefaults.StandardUserDefaults.SetString(token, "FirebaseToken");


                    InstanceId.Notifications.ObserveTokenRefresh((sender, e) =>
                    {
                        // Note that this callback will be fired everytime a new token is generated, including the first
                        // time. So if you need to retrieve the token as soon as it is available this is where that
                        // should be done.
                        var refreshedToken = InstanceId.SharedInstance.Token;
                        Console.WriteLine("FCM device token refreshed is - " + token);
						NSUserDefaults.StandardUserDefaults.SetString(refreshedToken, "FirebaseToken");

                        // Do your magic to refresh the token where is needed
                    });

                }
            });
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            // Get current device token
            var DeviceToken = deviceToken.Description;
            if (!string.IsNullOrWhiteSpace(DeviceToken))
            {
                DeviceToken = DeviceToken.Trim('<').Trim('>');
            }

            Console.WriteLine("APNS device token is - " + DeviceToken);

            // Get previous device token
            var oldDeviceToken = NSUserDefaults.StandardUserDefaults.StringForKey("PushDeviceToken");

            // Has the token changed?
            if (string.IsNullOrEmpty(oldDeviceToken) || !oldDeviceToken.Equals(DeviceToken))
            {
                //TODO: Put your own logic here to notify your server that the device token has changed/been created!
            }

            // Save new device token 
            NSUserDefaults.StandardUserDefaults.SetString(DeviceToken, "PushDeviceToken");
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            new UIAlertView("Error registering push notifications", error.LocalizedDescription, null, "OK", null).Show();
        }



    }
}

