using System;

using Foundation;
using UIKit;
using FirebaseXamarin.Core.Utils;

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
			lblDateTime.Text = Utils.getFormmatedTime(message.timestamp).ToString("g");
			lblUserName.Text = DBManager.sharedManager.getLoggedInUserInfo().name;
		}
	}
}
