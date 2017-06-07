using Foundation;
using System;
using UIKit;
using Google.SignIn;
using System.Threading.Tasks;
using Firebase.Database;
using System.Collections.Generic;
using FirebaseXamarin.iOS.Delegates;
using FirebaseXamarin.Core.Utils;
using System.Linq;

namespace FirebaseXamarin.iOS
{
	public partial class UsersViewController : BaseViewController
	{
		UsersListDatasource userListDataSource;
		bool isGroupClicked = false;
		UserListDelegate userListDelegate;

		List<User> arrUsersForNewGroup;
		List<User> arrUsers;
		UIRefreshControl refreshControl = new UIRefreshControl();

		#region Setter
		void SetisGroupClicked(bool status)
		{
			if (this.isGroupClicked == status)
			{
				var selectedRows = tblViewUsers.IndexPathsForSelectedRows;
				List<User> selectedUsers = new List<User>();

				if (selectedRows == null || selectedRows.Count() <= 0)
				{
					ShowAlert("Message", "Please select more than one users", "Ok");
				}
				else
				{
					foreach (NSIndexPath indexPath in selectedRows)
					{
						selectedUsers.Add(arrUsers[indexPath.Row]);
					}
					proceedToCreateGroup();
					reset();
					this.isGroupClicked = false;
				}
				return;
			}

			this.isGroupClicked = status;
			if (status)
			{
				tblViewUsers.SetEditing(true, true);
				addCloseButton();

				btnNewGroup.Title = "Done";

				UsersListDatasource dataSourcenewGroup = new UsersListDatasource(arrUsersForNewGroup);
				tblViewUsers.DataSource = dataSourcenewGroup;
				userListDelegate = new UserListDelegate(this, arrUsersForNewGroup);
				userListDelegate.didSelectRowCallBack = (NSIndexPath indexPath) =>
									   {

									   };
				userListDelegate.isForGroupSelectiion = true;
				tblViewUsers.Delegate = userListDelegate;
				tblViewUsers.ReloadData();
			}
			else
			{
				tblViewUsers.SetEditing(false, true);
				reset();
			}
		}
		#endregion

		public UsersViewController(IntPtr handle) : base(handle)
		{

		}

		#region View Life Cycle
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			configureUI();
			showLoading("fetching users ...");
			fetchUsersAndDisplay();
		}

		public override void ViewWillAppear(bool animated)
		{

		}
		#endregion

		#region Private Methods
		private void configureUI()
		{
			NavigationController.NavigationBarHidden = false;
			NavigationController.NavigationBar.TintColor = UIColor.Black;
			btnNewGroup.Clicked += (sender, e) =>
			{
				SetisGroupClicked(true);
			};
			tblViewUsers.TableFooterView = new UIView();

			refreshControl.ValueChanged += (sender, e) =>
			{
				fetchUsersAndDisplay();
			};

			tblViewUsers.RefreshControl = refreshControl;
			tblViewUsers.AllowsMultipleSelectionDuringEditing = true;

			customiseNewGroupButton();
		}

		private void customiseNewGroupButton()
		{
			btnNewGroup.Title = "New Group";
			btnNewGroup.Style = UIBarButtonItemStyle.Plain;
		}

		private void addCloseButton()
		{
			UIBarButtonItem leftCancelButton = new UIBarButtonItem(UIBarButtonSystemItem.Cancel);
			leftCancelButton.Clicked += (sender, e) =>
			{
				SetisGroupClicked(false);
			};
			NavigationItem.SetLeftBarButtonItem(leftCancelButton, true);
		}

		private void removeCloseButton()
		{
			NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem(), true);
		}

		private void fetchUsersAndDisplay()
		{
			FirebaseManager.sharedManager.getAllUser((List<User> users) =>
			{
				hideLoading();
				InvokeOnMainThread(() =>
				{
					arrUsersForNewGroup = users.Select(item => (User)item.Clone()).ToList();
					arrUsers = users;
					if (users != null && users.Count > 0)
					{
						displayUsers(arrUsers);
					}
					else
					{
						ShowAlert("Message", "No users found", "Ok");
					}
					refreshControl.EndRefreshing();
				});
			});
		}

		private void reset()
		{
			customiseNewGroupButton();
			removeCloseButton();
			displayUsers(arrUsers);
			tblViewUsers.SetEditing(false, true);
		}

		private void proceedToCreateGroup()
		{
			UIAlertView alert = new UIAlertView();
			alert.Title = "Please enter group name";
			alert.AddButton("Confirm");
			alert.AddButton("Cancel");
			alert.AlertViewStyle = UIAlertViewStyle.PlainTextInput;
			alert.Show();
			alert.Clicked += (sender, e) =>
			{
				nint buttonIndex = e.ButtonIndex;
				if (buttonIndex == 0)
				{
					//Confirmed
					string groupName = alert.GetTextField(0).Text.Trim();
					//1.Create/Log a room between you and selected user 2.Move to Chat screen 
					RoomsMetaData roomMetaData = new RoomsMetaData();
					roomMetaData.type = Constants.ROOM_TYPE_GROUP;
					roomMetaData.createdBy = DBManager.sharedManager.getLoggedInUserInfo().uid;
					roomMetaData.createdTime = Utils.getCurrentTime(); ;
					roomMetaData.lastUpdatedTime = Utils.getCurrentTime();
					roomMetaData.displayName = groupName;

					List<User> arrSelectedUsers = arrUsersForNewGroup.Where(item => item.isSelected == true).ToList();
					roomMetaData.users = new List<string>(arrSelectedUsers.Select(item => item.uid));
					roomMetaData.users.Add(DBManager.sharedManager.getLoggedInUserInfo().uid);

					roomMetaData.arrFireBaseTokens = new List<string>(arrSelectedUsers.Select(item => item.firebaseToken));

					showLoading("Creating Room ...");
					FirebaseManager.sharedManager.createGroup(roomMetaData, (string roomId) =>
					{
						hideLoading();
						roomMetaData.roomId = roomId;
						ShowAlert("Message", "Group created successfully", "Ok");
						reset();
					});
				}
				else
				{
					reset();
				}

			};
		}

		void displayUsers(List<User> users)
		{
			userListDataSource = new UsersListDatasource(users);
			tblViewUsers.DataSource = userListDataSource;

			userListDelegate = new UserListDelegate(this, users);
			tblViewUsers.Delegate = userListDelegate;
			tblViewUsers.ReloadData();
		}

		#endregion

		#region UserListActionProtocol Methods

		#endregion
	}
}