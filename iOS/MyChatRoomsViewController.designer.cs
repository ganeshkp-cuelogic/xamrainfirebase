// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace FirebaseXamarin.iOS
{
    [Register ("MyChatRoomsViewController")]
    partial class MyChatRoomsViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView tblViewChatRooms { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (tblViewChatRooms != null) {
                tblViewChatRooms.Dispose ();
                tblViewChatRooms = null;
            }
        }
    }
}