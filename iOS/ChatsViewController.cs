using Foundation;
using System;
using UIKit;
using System.Collections.Generic;
using AVFoundation;
using FirebaseXamarin.Core.Utils;

namespace FirebaseXamarin.iOS
{
    public partial class ChatsViewController : BaseViewController
    {
        private RoomsMetaData roomMeataData;
        NSObject notification;

        public void setRoomMetaData(RoomsMetaData roomMetaData)
        {
            this.roomMeataData = roomMetaData;
        }

        public ChatsViewController(IntPtr handle) : base(handle)
        {

        }

        #region View Life Cycle
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            configureUI();
            fetchMessages();
        }

        public override void ViewDidDisappear(bool animated)
        {
            stopObservingKeyboard();
        }
        #endregion

        #region Private Methods
        private void configureUI()
        {
            NavigationController.NavigationBarHidden = false;
            NavigationController.NavigationBar.TintColor = UIColor.Black;
            Title = roomMeataData.displayName;

            bottomConstraint.Constant = 0;
            heightConstraintSendView.Constant = 47;
            View.LayoutIfNeeded();
            SetupKeyboardObserver();

            tblViewChats.RegisterNibForCellReuse(UINib.FromName("LeftChatMessageCell", NSBundle.MainBundle), LeftChatMessageCell.Key);
            tblViewChats.RegisterNibForCellReuse(UINib.FromName("RightChatMessageCell", NSBundle.MainBundle), RightChatMessageCell.Key);
            tblViewChats.RowHeight = UITableView.AutomaticDimension;
            tblViewChats.EstimatedRowHeight = new nfloat(100.0);

            btnSend.TouchUpInside += (sender, e) =>
            {
                validateMessage();
                sendMessage();
                View.EndEditing(true);
                configureTextView();
            };

            tblViewChats.AddGestureRecognizer(new UITapGestureRecognizer((UITapGestureRecognizer obj) =>
            {
                View.EndEditing(true);
            }));

            configureTextView();
        }

        void configureTextView()
        {
            var Placeholder = "Enter Message";
            txtViewChatMessage.Text = Placeholder;
            txtViewChatMessage.ShouldBeginEditing = t =>
            {
                if (txtViewChatMessage.Text == Placeholder)
                    txtViewChatMessage.Text = string.Empty;

                return true;
            };

            txtViewChatMessage.ShouldEndEditing = t =>
            {
                if (string.IsNullOrEmpty(txtViewChatMessage.Text))
                    txtViewChatMessage.Text = Placeholder;
                return true;
            };
        }

        private void moveToEnd()
        {

        }

        private bool validateMessage()
        {
            bool status = true;
            if (txtViewChatMessage.Text.Trim(' ').Length == 0)
            {
                status = false;
                ShowAlert("Alert", "Please enter the message", "Ok");
            }

            return status;
        }

        private void sendMessage()
        {
            //Hit send message API
            Message message = new Message();
            message.message = txtViewChatMessage.Text.Trim(' ');
            message.timestamp = Utils.getCurrentTime();
            message.roomId = roomMeataData.roomId;
            message.sender_id = DBManager.sharedManager.getLoggedInUserInfo().uid;
            FirebaseManager.sharedManager.sendMessage(message);
        }

        private void fetchMessages()
        {
            showLoading("Fetching Messages ...");
            FirebaseManager.sharedManager.getAllRoomMessages(roomMeataData.roomId, (List<Message> messages) =>
            {
                InvokeOnMainThread(() =>
                {
                    hideLoading();
                    if (messages != null && messages.Count > 0)
                    {
                        ChatMessagesDatasource chatDataSource = new ChatMessagesDatasource(messages);
                        tblViewChats.DataSource = chatDataSource;
                        tblViewChats.ReloadData();
                        tblViewChats.ScrollToRow(NSIndexPath.FromRowSection(messages.Count - 1, 0), UITableViewScrollPosition.Bottom, true);
                    }
                    else
                    {
                        ShowAlert("Message", "No messages found", "Ok");
                    }
                });
            });
        }
        #endregion

        #region Observe Keyboard
        void SetupKeyboardObserver()
        {
            // listening
            notification = UIKeyboard.Notifications.ObserveWillShow((sender, args) =>
            {
                /* Access strongly typed args */
                Console.WriteLine("Notification: {0}", args.Notification);

                Console.WriteLine("FrameBegin", args.FrameBegin);
                Console.WriteLine("FrameEnd", args.FrameEnd);
                Console.WriteLine("AnimationDuration", args.AnimationDuration);
                Console.WriteLine("AnimationCurve", args.AnimationCurve);

                UIView.Animate(args.AnimationDuration, () =>
                {
                    bottomConstraint.Constant = args.FrameEnd.Size.Height;
                }, () =>
                {

                });
            });

            notification = UIKeyboard.Notifications.ObserveWillHide((sender, args) =>
            {
                /* Access strongly typed args */
                Console.WriteLine("Notification: {0}", args.Notification);

                Console.WriteLine("FrameBegin", args.FrameBegin);
                Console.WriteLine("FrameEnd", args.FrameEnd);
                Console.WriteLine("AnimationDuration", args.AnimationDuration);
                Console.WriteLine("AnimationCurve", args.AnimationCurve);

                UIView.Animate(args.AnimationDuration, () =>
                {
                    bottomConstraint.Constant = 0;
                }, () =>
                {

                });
            });
        }

        void stopObservingKeyboard()
        {
            notification.Dispose();
        }
        #endregion
    }
}