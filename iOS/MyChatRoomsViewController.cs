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

			fetchAllRooms();
		}

		private void configureUI()
		{
			tblViewChatRooms.RegisterNibForCellReuse(UINib.FromName("ChatRoomsCell", NSBundle.MainBundle), ChatRoomsCell.Key);
			tblViewChatRooms.TableFooterView = new UIView();
			TabBarController.NavigationController.NavigationBarHidden = true;
			NavigationController.NavigationBarHidden = false;
			NavigationController.Title = "Chats";

			//Logout Button
			NavigationController.NavigationBarHidden = false;
			UIBarButtonItem rightBarButtonItem = new UIBarButtonItem(UIImage.FromBundle("logout"), UIBarButtonItemStyle.Plain, (sender, e) =>
			{
				SignIn.SharedInstance.SignOutUser();
				AppDelegate.applicationDelegate().moveToLoginScreen();
				DBManager.sharedManager.deleteUserInfo();
			});
			NavigationItem.SetRightBarButtonItem(rightBarButtonItem, true);
			NavigationController.NavigationBar.TintColor = UIColor.Black;
		}

		private void fetchAllRooms()
		{
			showLoading("Fetching chats ...");
			FirebaseManager.sharedManager.fetchAllMyRooms("aTtKcDCfkben2wfyycNcm0ihCNk1", (rooms) =>
			{
				InvokeOnMainThread(() =>
				{
					hideLoading();
					chatRoomsDataSource = new MyChatRoomsDatasource(rooms);
					tblViewChatRooms.Source = chatRoomsDataSource;
					tblViewChatRooms.Delegate = new MyChatRoomsDelegate(NavigationController, rooms);
					tblViewChatRooms.ReloadData();
				});
			});
		}
	}
}