using Foundation;
using System;
using UIKit;
using FirebaseXamarin.iOS.Datasources;
using FirebaseXamarin.iOS.Cells;

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
                    tblViewChatRooms.ReloadData();
                });
            });
        }
    }
}