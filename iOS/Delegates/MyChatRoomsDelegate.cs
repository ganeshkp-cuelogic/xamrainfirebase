using System;
using UIKit;
using System.Collections.Generic;
namespace FirebaseXamarin.iOS
{
	public class MyChatRoomsDelegate : UITableViewDelegate
	{
		UINavigationController navigationController;
		List<RoomsMetaData> rooms;
		ChatsViewController chatsViewController;

		public MyChatRoomsDelegate()
		{

		}

		public MyChatRoomsDelegate(UINavigationController navigationController, List<RoomsMetaData> rooms)
		{
			this.navigationController = navigationController;
			this.rooms = rooms;
		}

		public override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			RoomsMetaData roomMetaData = rooms[indexPath.Row];
			chatsViewController = (ChatsViewController)navigationController.Storyboard.InstantiateViewController("ChatsViewController");
			chatsViewController.HidesBottomBarWhenPushed = true;
			chatsViewController.setRoomMetaData(roomMetaData);
			navigationController.PushViewController(chatsViewController, true);
		}
	}
}
