using System;
using UIKit;
namespace FirebaseXamarin.iOS.Delegates
{
	public class UserListDelegate: UITableViewDelegate
    {
		UIViewController listViewController;
        public UserListDelegate()
        {
        }

		public UserListDelegate(UIViewController listViewController) 
		{
			this.listViewController = listViewController;
		}

		public override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			base.RowSelected(tableView, indexPath);
			ChatsViewController chatsViewController = (ChatsViewController)listViewController.Storyboard.InstantiateViewController("ChatsViewController");
			listViewController.NavigationController.PushViewController(chatsViewController, true);
		}

    }
}
