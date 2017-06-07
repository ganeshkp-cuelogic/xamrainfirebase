using Foundation;
using System;
using UIKit;
using FirebaseXamarin.iOS.Datasources;
using FirebaseXamarin.iOS.Cells;
using Google.SignIn;
using System.Collections.Generic;
using System.Linq;

namespace FirebaseXamarin.iOS
{
	public partial class MyChatRoomsViewController : BaseViewController
	{
		MyChatRoomsDatasource chatRoomsDataSource;
		UIRefreshControl refreshControl = new UIRefreshControl();

		public MyChatRoomsViewController(IntPtr handle) : base(handle)
		{

		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			configureUI();
			showLoading("Fetching chats ...");
			fetchAllRooms();
		}

		public override void ViewWillAppear(bool animated)
		{

		}

		public override void ViewWillDisappear(bool animated)
		{

		}

		private void configureUI()
		{
			tblViewChatRooms.RegisterNibForCellReuse(UINib.FromName("ChatRoomsCell", NSBundle.MainBundle), ChatRoomsCell.Key);
			tblViewChatRooms.TableFooterView = new UIView();
			TabBarController.NavigationController.NavigationBarHidden = true;
			NavigationController.NavigationBarHidden = false;
			NavigationController.Title = "Chats";

			NavigationController.NavigationBar.TintColor = UIColor.Black;

			refreshControl.ValueChanged += (sender, e) =>
			{
				fetchAllRooms();
			};
			tblViewChatRooms.RefreshControl = refreshControl;
		}

		private void fetchAllRooms()
		{
			FirebaseManager.sharedManager.fetchAllMyRooms(DBManager.sharedManager.getLoggedInUserInfo().uid, (rooms) =>
			{
				InvokeOnMainThread(() =>
				{
					hideLoading();
					rooms = rooms.OrderByDescending(x => x.lastUpdatedTime).ToList();
					if (rooms != null && rooms.Count > 0)
					{
						chatRoomsDataSource = new MyChatRoomsDatasource(rooms);
						tblViewChatRooms.Source = chatRoomsDataSource;
						tblViewChatRooms.Delegate = new MyChatRoomsDelegate(NavigationController, rooms);
					}
					else
					{
						ShowAlert("Message", "No chats found", "Ok");
						chatRoomsDataSource = new MyChatRoomsDatasource(new List<RoomsMetaData>());
						tblViewChatRooms.Source = chatRoomsDataSource;
					}
					tblViewChatRooms.ReloadData();
					refreshControl.EndRefreshing();
				});
			});
		}
	}
}