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
        }
    }
}