using Foundation;
using System;
using UIKit;
using FirebaseXamarin.iOS.Datasources;
using FirebaseXamarin.iOS.Cells;
using Google.SignIn;

namespace FirebaseXamarin.iOS
{
	public partial class MyChatRoomsViewController : BaseViewController
	{

		MyChatRoomsDatasource chatRoomsDataSource;

		public MyChatRoomsViewController(IntPtr handle) : base(handle)
		{

		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			configureUI();
		}

		public override void ViewWillAppear(bool animated)
		{
			fetchAllRooms();
		}

		private void configureUI()
		{
			tblViewChatRooms.RegisterNibForCellReuse(UINib.FromName("ChatRoomsCell", NSBundle.MainBundle), ChatRoomsCell.Key);
			tblViewChatRooms.TableFooterView = new UIView();
			TabBarController.NavigationController.NavigationBarHidden = true;
			NavigationController.NavigationBarHidden = false;
			NavigationController.Title = "Chats";

			NavigationController.NavigationBar.TintColor = UIColor.Black;
		}

		private void fetchAllRooms()
		{
			showLoading("Fetching chats ...");
			FirebaseManager.sharedManager.fetchAllMyRooms(DBManager.sharedManager.getLoggedInUserInfo().uid, (rooms) =>
			{
				InvokeOnMainThread(() =>
				{
					hideLoading();
					if (rooms != null && rooms.Count > 0)
					{
						chatRoomsDataSource = new MyChatRoomsDatasource(rooms);
						tblViewChatRooms.Source = chatRoomsDataSource;
						tblViewChatRooms.Delegate = new MyChatRoomsDelegate(NavigationController, rooms);
						tblViewChatRooms.ReloadData();
					}
					else
					{
						ShowAlert("Message", "No chats found", "Ok");
					}
				});
			});
		}
	}
}