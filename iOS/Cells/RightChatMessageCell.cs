using System;

using Foundation;
using UIKit;
using FirebaseXamarin.Core.Utils;
using SDWebImage;

namespace FirebaseXamarin.iOS
{
	public partial class RightChatMessageCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString("RightChatMessageCell");
		public static readonly UINib Nib;

		static RightChatMessageCell()
		{
			Nib = UINib.FromName("RightChatMessageCell", NSBundle.MainBundle);
		}

		protected RightChatMessageCell(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public void populateData(Message message)
		{
			lblMessage.Text = message.message;
			lblDateTime.Text = Utils.getFormmatedTime(message.timestamp).ToString(Constants.DISPLAY_DATE_FORMAT);
			lblUserName.Text = DBManager.sharedManager.getLoggedInUserInfo().name;

			imgViewProfile.Layer.CornerRadius = imgViewProfile.Frame.Size.Width / 2;
			imgViewProfile.ClipsToBounds = true;
			imgViewProfile.SetImage(
					url: new NSUrl(DBManager.sharedManager.getLoggedInUserInfo().profilePic),
				placeholder: UIImage.FromBundle("user.png"));
		}
	}
}
