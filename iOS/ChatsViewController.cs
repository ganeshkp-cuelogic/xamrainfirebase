using Foundation;
using System;
using UIKit;
using System.Collections.Generic;
using AVFoundation;

namespace FirebaseXamarin.iOS
{
    public partial class ChatsViewController : BaseViewController
    {
        private RoomsMetaData roomMeataData;

        public void setRoomMetaData(RoomsMetaData roomMetaData)
        {
            this.roomMeataData = roomMetaData;
        }

        public ChatsViewController(IntPtr handle) : base(handle)
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            configureUI();
            fetchMessages();
        }

        private void configureUI()
        {
            NavigationController.NavigationBarHidden = false;
            NavigationController.NavigationItem.Title = roomMeataData.displayName;
            tblViewChats.RegisterNibForCellReuse(UINib.FromName("LeftChatMessageCell", NSBundle.MainBundle), LeftChatMessageCell.Key);
            tblViewChats.RegisterNibForCellReuse(UINib.FromName("RightChatMessageCell", NSBundle.MainBundle), RightChatMessageCell.Key);
        }

        private void fetchMessages()
        {
            showLoading("Fetching Messages ...");
            FirebaseManager.sharedManager.getAllRoomMessages(roomMeataData.roomId, (List<Message> messages) =>
            {
                InvokeOnMainThread(() =>
                {
                    hideLoading();
                    ChatMessagesDatasource chatDataSource = new ChatMessagesDatasource(messages);
                    tblViewChats.DataSource = chatDataSource;
                    tblViewChats.ReloadData();
                });
            });
        }
    }
}