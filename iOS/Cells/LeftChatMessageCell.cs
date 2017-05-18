using System;

using Foundation;
using UIKit;
using FirebaseXamarin.Core.Utils;

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
			lblMessage.Text = message.message;
			lblDateTime.Text = Utils.getFormmatedTime(message.timestamp).ToString("g");
			FirebaseManager.sharedManager.getUserInfo(message.sender_id, (User userInfo) =>
			{
				lblUserName.Text = userInfo.name;
			});
		}
	}
}
