using Foundation;
using System;
using UIKit;
using Google.SignIn;
using CoreGraphics;
using Firebase.Auth;
using Firebase.Database;

namespace FirebaseXamarin.iOS
{
    public partial class LoginViewController : BaseViewController, ISignInDelegate, ISignInUIDelegate
    {

        public SignInButton SignInBtn { get; private set; }

        public LoginViewController(IntPtr handle) : base(handle)
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            confugureUI();
        }

        private void confugureUI()
        {

            SignInBtn = new SignInButton();
            SignInBtn.Frame = new CGRect(20, 100, 150, 44);
            SignInBtn.Center = View.Center;
            View.AddSubview(SignInBtn);

            // Assign the SignIn Delegates to receive callbacks
            SignIn.SharedInstance.UIDelegate = this;
            SignIn.SharedInstance.Delegate = this;

        }

        public void DidSignIn(SignIn signIn, GoogleUser user, NSError error)
        {
            if (user != null && error == null)
            {


                string emailID = user.Profile.Email;
                if (emailID.EndsWith("cuelogic.co.in") || emailID.EndsWith("cuelogic.co.in"))
                {
                    // Do your magic to handle authentication result
                }
                else
                {
                    ShowAlert("Message", "Please login using Cuelogic email id", "Ok");
                    return;
                }

                // Disable the SignInButton
                GPLoadingIndicator.showLoading(AppDelegate.applicationDelegate().Window.Bounds, "Signing in ...");

                //Hit the Auth API
                // Get Google ID token and Google access token and exchange them for a Firebase credential
                var authentication = user.Authentication;
                var credential = GoogleAuthProvider.GetCredential(authentication.IdToken, authentication.AccessToken);

                // Authenticate with Firebase using the credential
                // Visit https://firebase.google.com/docs/auth/ios/errors for more information
                Auth.DefaultInstance.SignIn(credential, (userInfo, errorInfo) =>
                {
                    GPLoadingIndicator.Hide();

                    if (errorInfo != null)
                    {
                        AuthErrorCode errorCode;
                        if (IntPtr.Size == 8) // 64 bits devices
                            errorCode = (AuthErrorCode)((long)errorInfo.Code);
                        else // 32 bits devices
                            errorCode = (AuthErrorCode)((int)errorInfo.Code);

                        // Posible error codes that SignIn method with credentials could throw
                        switch (errorCode)
                        {
                            case AuthErrorCode.InvalidCredential:
                            case AuthErrorCode.InvalidEmail:
                            case AuthErrorCode.OperationNotAllowed:
                            case AuthErrorCode.EmailAlreadyInUse:
                            case AuthErrorCode.UserDisabled:
                            case AuthErrorCode.WrongPassword:
                            default:
                                // Print error
                                break;
                        }
                    }
                    else
                    {
                        // ShowAlert("Message", "Login Successfull", "Ok");
                        AppDelegate.applicationDelegate().moveToUsersScreen();
                        User userModel = new User();
                        userModel.emailid = userInfo.Email;
                        userModel.uid = userInfo.Uid;
                        userModel.name = userInfo.DisplayName;
                        userModel.profilePic = userInfo.PhotoUrl.AbsoluteString;
                        userModel.firebaseToken = NSUserDefaults.StandardUserDefaults.StringForKey("FirebaseToken");
                        DBManager.sharedManager.saveUserInfo(userModel);

                        //Add the user in firebase database
                        DatabaseReference rootNode = Database.DefaultInstance.GetRootReference();
                        DatabaseReference userNode = rootNode.GetChild("users").GetChild(userModel.uid);
                        userNode.SetValue<NSDictionary>(userModel.ToDictionary());
                    }

                });
            }
        }

        [Export("signInWillDispatch:error:")]
        public void WillDispatch(SignIn signIn, NSError error)
        {

        }

        [Export("signIn:presentViewController:")]
        public void PresentViewController(SignIn signIn, UIViewController viewController)
        {
            PresentViewController(viewController, true, null);
        }

        [Export("signIn:dismissViewController:")]
        public void DismissViewController(SignIn signIn, UIViewController viewController)
        {
            DismissViewController(true, null);
        }
    }
}