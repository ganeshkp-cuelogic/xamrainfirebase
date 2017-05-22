using System;
using UIKit;
using FirebaseXamarin.Core.Utils;
using System.Collections.Generic;

namespace FirebaseXamarin.iOS.Delegates
{
    public class UserListDelegate : UITableViewDelegate
    {
        UIViewController listViewController;
        ChatsViewController chatsViewController;
        List<User> users;
        public UserListDelegate()
        {

        }

        public UserListDelegate(UIViewController listViewController, List<User> users)
        {
            this.listViewController = listViewController;
            this.users = users;
        }

        public override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            User userSelected = users[indexPath.Row];

            //1.Create/Log a room between you and selected user 2.Move to Chat screen 
            RoomsMetaData roomMetaData = new RoomsMetaData();
            roomMetaData.type = Constants.ROOM_TYPE_ONE_ONE;
            roomMetaData.createdBy = DBManager.sharedManager.getLoggedInUserInfo().uid;
            roomMetaData.createdTime = Utils.getCurrentTime(); ;
            roomMetaData.lastUpdatedTime = Utils.getCurrentTime();
            roomMetaData.users = new List<string>() { userSelected.uid, DBManager.sharedManager.getLoggedInUserInfo().uid };
            FirebaseManager.sharedManager.createGroup(roomMetaData, (string roomId) =>
            {
                roomMetaData.roomId = roomId;
                chatsViewController = (ChatsViewController)listViewController.Storyboard.InstantiateViewController("ChatsViewController");
                chatsViewController.HidesBottomBarWhenPushed = true;
                chatsViewController.setRoomMetaData(roomMetaData);
                listViewController.NavigationController.PushViewController(chatsViewController, true);
            });
        }
    }
}
