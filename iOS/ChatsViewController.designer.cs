// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace FirebaseXamarin.iOS
{
    [Register ("ChatsViewController")]
    partial class ChatsViewController
    {
        [Outlet]
        UIKit.NSLayoutConstraint bottomConstraint { get; set; }


        [Outlet]
        UIKit.UIButton btnSend { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView tblViewChats { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        FirebaseXamarin.iOS.GPTextView txtViewChatMessage { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (bottomConstraint != null) {
                bottomConstraint.Dispose ();
                bottomConstraint = null;
            }

            if (btnSend != null) {
                btnSend.Dispose ();
                btnSend = null;
            }

            if (tblViewChats != null) {
                tblViewChats.Dispose ();
                tblViewChats = null;
            }

            if (txtViewChatMessage != null) {
                txtViewChatMessage.Dispose ();
                txtViewChatMessage = null;
            }
        }
    }
}