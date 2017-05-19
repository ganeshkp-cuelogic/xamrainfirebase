using System;

using Foundation;
using UIKit;
using FirebaseXamarin.Core.Utils;
using SDWebImage;

namespace FirebaseXamarin.iOS
{
	public partial class LeftChatMessageCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString("LeftChatMessageCell");
		public static readonly UINib Nib;

		static LeftChatMessageCell()
		{
			Nib = UINib.FromName("LeftChatMessageCell", NSBundle.MainBundle);
		}

		protected LeftChatMessageCell(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public void populateData(Message message)
		{
			imgViewProfile.Layer.CornerRadius = imgViewProfile.Frame.Size.Width / 2;
			imgViewProfile.ClipsToBounds = true;

			lblMessage.Text = message.message;
			lblDateTime.Text = Utils.getFormmatedTime(message.timestamp).ToString(Constants.DISPLAY_DATE_FORMAT);
			FirebaseManager.sharedManager.getUserInfo(message.sender_id, (User userInfo) =>
			{
				lblUserName.Text = userInfo.name;
				imgViewProfile.SetImage(
					url: new NSUrl(userInfo.profilePic),
				placeholder: UIImage.FromBundle("user.png")
			);
			});
		}
	}
}
