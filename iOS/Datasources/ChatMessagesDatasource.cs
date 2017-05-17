using System;
using UIKit;
using System.Collections.Generic;
namespace FirebaseXamarin.iOS
{
    public class ChatMessagesDatasource : UITableViewDataSource
    {
        List<Message> messages;
        public ChatMessagesDatasource()
        {

        }

        public ChatMessagesDatasource(List<Message> messages)
        {
            this.messages = messages;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return this.messages.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            Message message = messages[indexPath.Row];

            if (message.sender_id != DBManager.sharedManager.getLoggedInUserInfo().uid)
            { //Which Mean sender is me
                LeftChatMessageCell leftChatCell = (LeftChatMessageCell)tableView.DequeueReusableCell(LeftChatMessageCell.Key, indexPath);
                leftChatCell.populateData(message);
                return leftChatCell;
            }

            RightChatMessageCell rightCell = (RightChatMessageCell)tableView.DequeueReusableCell(RightChatMessageCell.Key, indexPath);
            rightCell.populateData(message);
            return rightCell;
        }
    }
}
