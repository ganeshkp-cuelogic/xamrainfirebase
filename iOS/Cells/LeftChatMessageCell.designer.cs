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
	[Register ("LeftChatMessageCell")]
	partial class LeftChatMessageCell
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIImageView imgViewProfile { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel lblDateTime { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel lblMessage { get; set; }

		[Outlet]
		UIKit.UILabel lblUserName { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (imgViewProfile != null) {
				imgViewProfile.Dispose ();
				imgViewProfile = null;
			}

			if (lblDateTime != null) {
				lblDateTime.Dispose ();
				lblDateTime = null;
			}

			if (lblMessage != null) {
				lblMessage.Dispose ();
				lblMessage = null;
			}

			if (lblUserName != null) {
				lblUserName.Dispose ();
				lblUserName = null;
			}
		}
	}
}
