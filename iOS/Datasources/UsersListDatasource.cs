using System;
using Foundation;
using UIKit;
using System.Collections.Generic;
namespace FirebaseXamarin.iOS
{
    public class UsersListDatasource : UITableViewDataSource
    {

        List<User> users;

        public UsersListDatasource()
        {
        }

        public UsersListDatasource(List<User> users)
        {
            this.users = users;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell("cell", indexPath);

            User user = users[indexPath.Row];
            UILabel labelName = (UILabel)cell.ContentView.ViewWithTag(100);
            labelName.Text = user.name;

            return cell;
        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            return users.Count;
        }

    }
}
