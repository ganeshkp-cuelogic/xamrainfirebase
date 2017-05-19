// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace FirebaseXamarin.iOS
{
	[Register("ChatsViewController")]
	partial class ChatsViewController
	{
		[Outlet]
		UIKit.NSLayoutConstraint bottomConstraint { get; set; }

		[Outlet]
		UIKit.UIButton btnSend { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint heightConstraintSendView { get; set; }

		[Outlet]
		[GeneratedCode("iOS Designer", "1.0")]
		UIKit.UITableView tblViewChats { get; set; }

		[Outlet]
		[GeneratedCode("iOS Designer", "1.0")]
		FirebaseXamarin.iOS.GPTextView txtViewChatMessage { get; set; }

		void ReleaseDesignerOutlets()
		{
			if (btnSend != null)
			{
				btnSend.Dispose();
				btnSend = null;
			}

			if (heightConstraintSendView != null)
			{
				heightConstraintSendView.Dispose();
				heightConstraintSendView = null;
			}

			if (tblViewChats != null)
			{
				tblViewChats.Dispose();
				tblViewChats = null;
			}

			if (txtViewChatMessage != null)
			{
				txtViewChatMessage.Dispose();
				txtViewChatMessage = null;
			}

			if (bottomConstraint != null)
			{
				bottomConstraint.Dispose();
				bottomConstraint = null;
			}
		}
	}
}
