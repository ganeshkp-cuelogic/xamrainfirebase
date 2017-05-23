// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace FirebaseXamarin.iOS
{
    [Register ("UsersViewController")]
    partial class UsersViewController
    {
        [Outlet]
        UIKit.UIBarButtonItem btnDone { get; set; }


        [Outlet]
        UIKit.UIBarButtonItem btnNewGroup { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView tblViewUsers { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnNewGroup != null) {
                btnNewGroup.Dispose ();
                btnNewGroup = null;
            }

            if (tblViewUsers != null) {
                tblViewUsers.Dispose ();
                tblViewUsers = null;
            }
        }
    }
}