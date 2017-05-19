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
		bool isGroupClicked;

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
			NavigationController.NavigationBarHidden = false;
			btnNewGroup.Clicked += (sender, e) =>
			{
				isGroupClicked = true;
			};

			tblViewUsers.TableFooterView = new UIView();
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
						tblViewUsers.Delegate = new UserListDelegate(this, users);
						tblViewUsers.ReloadData();
					}
				});
			});
		}
	}
}