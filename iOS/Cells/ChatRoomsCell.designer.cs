// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace FirebaseXamarin.iOS.Cells
{
    [Register ("ChatRoomsCell")]
    partial class ChatRoomsCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imgViewProfile { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblChatRoomName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblDateTime { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblLastMessage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblMessageCount { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (imgViewProfile != null) {
                imgViewProfile.Dispose ();
                imgViewProfile = null;
            }

            if (lblChatRoomName != null) {
                lblChatRoomName.Dispose ();
                lblChatRoomName = null;
            }

            if (lblDateTime != null) {
                lblDateTime.Dispose ();
                lblDateTime = null;
            }

            if (lblLastMessage != null) {
                lblLastMessage.Dispose ();
                lblLastMessage = null;
            }

            if (lblMessageCount != null) {
                lblMessageCount.Dispose ();
                lblMessageCount = null;
            }
        }
    }
}