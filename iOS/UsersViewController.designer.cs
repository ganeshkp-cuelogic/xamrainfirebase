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
	[Register ("UsersViewController")]
	partial class UsersViewController
	{
		[Outlet]
		UIKit.UIBarButtonItem btnNewGroup { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UITableView tblViewUsers { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (tblViewUsers != null) {
				tblViewUsers.Dispose ();
				tblViewUsers = null;
			}

			if (btnNewGroup != null) {
				btnNewGroup.Dispose ();
				btnNewGroup = null;
			}
		}
	}
}
