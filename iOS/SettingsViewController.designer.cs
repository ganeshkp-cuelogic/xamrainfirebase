// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace FirebaseXamarin.iOS
{
    [Register ("SettingsViewController")]
    partial class SettingsViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnLogout { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imgViewProfile { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblAppVersion { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblEmailId { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwitch switchNotificationAlert { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnLogout != null) {
                btnLogout.Dispose ();
                btnLogout = null;
            }

            if (imgViewProfile != null) {
                imgViewProfile.Dispose ();
                imgViewProfile = null;
            }

            if (lblAppVersion != null) {
                lblAppVersion.Dispose ();
                lblAppVersion = null;
            }

            if (lblEmailId != null) {
                lblEmailId.Dispose ();
                lblEmailId = null;
            }

            if (lblName != null) {
                lblName.Dispose ();
                lblName = null;
            }

            if (switchNotificationAlert != null) {
                switchNotificationAlert.Dispose ();
                switchNotificationAlert = null;
            }
        }
    }
}