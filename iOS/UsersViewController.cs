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
            DatabaseReference rootNode = Database.DefaultInstance.GetRootReference();
            rootNode.KeepSynced(true);
            DatabaseReference userNode = rootNode.GetChild("users");
            userNode.ObserveEvent(DataEventType.Value, (snapshot) =>
            {
                hideLoading();
                // Loop over the children
                NSEnumerator children = snapshot.Children;
                var child = children.NextObject() as DataSnapshot;

                List<User> users = new List<User>();
                while (child != null)
                {
                    // Work with data...
                    var dictionaryData = child.GetValue<NSDictionary>();
                    if (dictionaryData["uid"].ToString() != DBManager.sharedManager.getLoggedInUserInfo().uid)
                    {
                        users.Add(User.fromDictionary(dictionaryData));
                    }

                    child = children.NextObject() as DataSnapshot;
                }

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