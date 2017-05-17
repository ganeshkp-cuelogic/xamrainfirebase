using Foundation;
using System;
using UIKit;
using Google.SignIn;
using System.Threading.Tasks;
using Firebase.Database;
using System.Collections.Generic;
using FirebaseXamarin.iOS.Delegates;

namespace FirebaseXamarin.iOS
{
    public partial class UsersViewController : BaseViewController
    {
        UsersListDatasource userListDataSource;
        public UsersViewController(IntPtr handle) : base(handle)
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            configureUI();
            fetchUsersAndDisplay();

            //TODO - Remove the Hard Coded Functions
            createGroups();
            fetchAllMessages();
        }

        private void configureUI()
        {
            NavigationController.NavigationBarHidden = false;
            UIBarButtonItem rightBarButtonItem = new UIBarButtonItem(UIImage.FromBundle("logout"), UIBarButtonItemStyle.Plain, (sender, e) =>
            {
                SignIn.SharedInstance.SignOutUser();
                AppDelegate.applicationDelegate().moveToLoginScreen();
                DBManager.sharedManager.deleteUserInfo();
            });

            NavigationItem.SetRightBarButtonItem(rightBarButtonItem, true);
            NavigationController.NavigationBar.TintColor = UIColor.Black;

            tblViewUsers.TableFooterView = new UIView();
            tblViewUsers.Delegate = new UserListDelegate(this);
        }

        private void fetchUsersAndDisplay()
        {
            showLoading("fetching users ...");
            FirebaseManager.sharedManager.getAllUser((List<User> users) =>
            {
                hideLoading();

                InvokeOnMainThread(() =>
                {
                    if (users.Count > 0)
                    {
                        userListDataSource = new UsersListDatasource(users);
                        tblViewUsers.DataSource = userListDataSource;
                        tblViewUsers.ReloadData();
                    }
                });
            });
        }

        private void createGroups()
        {
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            RoomsMetaData roomMetaData = new RoomsMetaData();
            roomMetaData.createdBy = "YKigRRuykkYBnWXf4r3DmtFaOAH3";
            roomMetaData.createdTime = unixTimestamp + "";
            roomMetaData.displayName = "The Happyboys - Xamarin Champs";
            roomMetaData.lastUpdatedTime = unixTimestamp + "";
            roomMetaData.users = new List<string>() { "VIGPpLLUhLP0klnv9prPEE4c88A3", "VWyzg1wV5Lhez2nhGpUaRI83c9c2", "Y921Ai9R2NUatWbJcrPp97OHAO83", "YKigRRuykkYBnWXf4r3DmtFaOAH3" };
            FirebaseManager.sharedManager.createGroup(roomMetaData);
        }

        private void fetchAllMessages()
        {
            FirebaseManager.sharedManager.getAllRoomMessages("-KjvElEWco5BrYVQoGDz", (List<Message> obj) =>
            {

            });
        }

    }
}