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

		void SetisGroupClicked(bool status)
		{
			if (this.isGroupClicked == status)
			{
				List<User> selectedUsers = arrUsersForNewGroup.Where(item => item.isSelected == true).ToList();
				if (selectedUsers.Count <= 0)
				{
					ShowAlert("Message", "Please select more than one users", "Ok");
				}
				else
				{
					proceedToCreateGroup();
					reset();
					this.isGroupClicked = false;
				}
				return;
			}

			this.isGroupClicked = status;
			if (status)
			{
				addCloseButton();
				btnNewGroup.Image = UIImage.FromBundle("checked");
				UsersListDatasource dataSourcenewGroup = new UsersListDatasource(arrUsersForNewGroup);
				tblViewUsers.DataSource = dataSourcenewGroup;
				userListDelegate = new UserListDelegate(this, arrUsersForNewGroup);
				userListDelegate.didSelectRowCallBack = (NSIndexPath indexPath) =>
									   {
										   User selectedUser = arrUsersForNewGroup[indexPath.Row];
										   selectedUser.isSelected = !selectedUser.isSelected;
										   tblViewUsers.ReloadData();
									   };
				userListDelegate.isForGroupSelectiion = true;
				tblViewUsers.Delegate = userListDelegate;
				tblViewUsers.ReloadData();
			}
			else
			{
				reset();
			}
		}

		void reset()
		{
			btnNewGroup.Image = UIImage.FromBundle("newgroup");
			removeCloseButton();
			displayUsers(arrUsers);
		}

		void proceedToCreateGroup()
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

		public UsersViewController(IntPtr handle) : base(handle)
		{

		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			configureUI();
		}

		public override void ViewWillAppear(bool animated)
		{
			fetchUsersAndDisplay();
		}

		private void configureUI()
		{
			NavigationController.NavigationBarHidden = false;
			btnNewGroup.Clicked += (sender, e) =>
			{
				SetisGroupClicked(true);
			};
			tblViewUsers.TableFooterView = new UIView();
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
			showLoading("fetching users ...");
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
				});
			});
		}

		#region UserListActionProtocol Methods

		#endregion
	}
}