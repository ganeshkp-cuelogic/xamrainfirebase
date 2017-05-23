using Foundation;
using System;
using UIKit;
using SDWebImage;
using Google.SignIn;

namespace FirebaseXamarin.iOS
{
	public partial class SettingsViewController : UITableViewController
	{
		public SettingsViewController(IntPtr handle) : base(handle)
		{

		}

		public override void ViewDidLoad()
		{
			configureUI();
		}

		public override void ViewWillAppear(bool animated)
		{
			populateUserData();
		}

		void populateUserData()
		{
			User currentUser = DBManager.sharedManager.getLoggedInUserInfo();
			lblName.Text = currentUser.name;
			lblEmailId.Text = currentUser.emailid;
			imgViewProfile.SetImage(
				url: new NSUrl(currentUser.profilePic),
												placeholder: UIImage.FromBundle("user.png")
											);
		}

		void configureUI()
		{
			imgViewProfile.Layer.CornerRadius = imgViewProfile.Frame.Size.Width / 2;
			imgViewProfile.ClipsToBounds = true;
			btnLogout.TouchUpInside += (sender, e) =>
			{
				SignIn.SharedInstance.SignOutUser();
				AppDelegate.applicationDelegate().moveToLoginScreen();
				DBManager.sharedManager.deleteUserInfo();
			};
		}
	}
}