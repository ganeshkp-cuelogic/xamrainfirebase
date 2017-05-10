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
        }

        private void configureUI()
        {
            Title = "Users";
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
    }
}