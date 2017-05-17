using System;
using UIKit;
namespace FirebaseXamarin.iOS
{
    public class MyChatRoomsDelegate : UITableViewDelegate
    {
        UINavigationController navigationController;
        public MyChatRoomsDelegate()
        {
        }

        public MyChatRoomsDelegate(UINavigationController navigationController)
        {
            this.navigationController = navigationController;
        }

        public override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
        {

        }
    }
}
