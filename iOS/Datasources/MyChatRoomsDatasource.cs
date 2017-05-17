using System;
using UIKit;
using System.Collections.Generic;
using FirebaseXamarin.iOS.Cells;
namespace FirebaseXamarin.iOS.Datasources
{
	public class MyChatRoomsDatasource : UITableViewSource
	{
		List<RoomsMetaData> arrMyRooms;

		public MyChatRoomsDatasource()
		{

		}

		public MyChatRoomsDatasource(List<RoomsMetaData> rooms)
		{
			this.arrMyRooms = rooms;
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			return 1;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return this.arrMyRooms.Count;
		}

		public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			ChatRoomsCell chatRoomCell = (ChatRoomsCell)tableView.DequeueReusableCell(ChatRoomsCell.Key, indexPath);

			RoomsMetaData roomMetaData = this.arrMyRooms[indexPath.Row];
			chatRoomCell.populateData(roomMetaData);

			return chatRoomCell;
		}
	}
}
