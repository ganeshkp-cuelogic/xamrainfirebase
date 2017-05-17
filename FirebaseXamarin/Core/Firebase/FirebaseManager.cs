using System;
using Firebase.Database;
using System.Collections.Generic;
using Foundation;
using Newtonsoft.Json;

namespace FirebaseXamarin
{
	public class FirebaseManager
	{
		public FirebaseManager() { }

		public static FirebaseManager sharedManager = new FirebaseManager();

		private readonly DatabaseReference rootNode = Database.DefaultInstance.GetRootReference();

		public void getAllUser(Action<List<User>> callback)
		{
			DatabaseReference userNode = rootNode.GetChild(FirebaseConstants.FB_USERS);
			userNode.ObserveEvent(DataEventType.Value, (snapshot) =>
				{
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
					callback(users);
				});
		}

		public void createGroup(RoomsMetaData roomMetatData)
		{
			DatabaseReference roomNode = rootNode.GetChild(FirebaseConstants.FB_ROOMS);
			roomNode.GetChildByAutoId().UpdateChildValues(roomMetatData.toDictionary());
		}

		public void fetchAllRooms(string currentUserId, Action<List<RoomsMetaData>> roomsCallback)
		{
			DatabaseReference roomNode = rootNode.GetChild(FirebaseConstants.FB_ROOMS);
			roomNode.ObserveEvent(DataEventType.Value, (snapshot) =>
				{
					// Loop over the children
					NSEnumerator children = snapshot.Children;
					var child = children.NextObject() as DataSnapshot;

					List<RoomsMetaData> arrRooms = new List<RoomsMetaData>();
					while (child != null)
					{
						var dictionaryData = child.GetValue<NSDictionary>();
						RoomsMetaData room = RoomsMetaData.fromDictionary(dictionaryData);
						room.roomId = child.Key;

						arrRooms.Add(room);
						child = children.NextObject() as DataSnapshot;
					}
					roomsCallback(arrRooms);
				});
		}

		public void fetchAllMyRooms(string myUserId, Action<List<RoomsMetaData>> roomsCallback)
		{
			List<RoomsMetaData> roomMetaDataList = new List<RoomsMetaData>();
			getUserInfo(myUserId, (User userInfo) =>
			{
				if (userInfo != null)
				{
					foreach (string roomID in userInfo.arrRoomId)
					{
						getRoomInfo(roomID, (RoomsMetaData roomInfo) =>
						{
							roomMetaDataList.Add(roomInfo);
							roomsCallback(roomMetaDataList);
						});
					}
				}
				else
				{
					roomsCallback(null);
				}
			});
		}

		public void getRoomInfo(string roomId, Action<RoomsMetaData> roomInfoCallBack)
		{
			DatabaseReference roomNode = rootNode.GetChild(FirebaseConstants.FB_ROOMS);
			DatabaseReference roomDetailsNode = roomNode.GetChild(roomId);
			roomDetailsNode.ObserveEvent(DataEventType.Value, (snapshot) =>
				{
					if (snapshot != null)
					{
						var dictionaryData = snapshot.GetValue<NSDictionary>();
						RoomsMetaData room = RoomsMetaData.fromDictionary(dictionaryData);
						roomInfoCallBack(room);
					}
					else
					{
						roomInfoCallBack(null);
					}
				});
		}

		public void getUserInfo(string userId, Action<User> userInfoCallBack)
		{
			DatabaseReference usersNode = rootNode.GetChild(FirebaseConstants.FB_USERS);
			DatabaseReference userDetailsNode = usersNode.GetChild(userId);
			userDetailsNode.ObserveEvent(DataEventType.Value, (snapshot) =>
				{
					if (snapshot != null)
					{
						var dictionaryData = snapshot.GetValue<NSDictionary>();
						User user = User.fromDictionary(dictionaryData);
						userInfoCallBack(user);
					}
					else
					{
						userInfoCallBack(null);
					}
				});
		}

		public void getAllRoomMessages(string roomId, Action<List<Message>> messagesCallback)
		{
			DatabaseReference messageNode = rootNode.GetChild(FirebaseConstants.FB_NODE_MESSAGES);
			DatabaseReference roomNode = messageNode.GetChild(roomId);
			roomNode.ObserveEvent(DataEventType.Value, (snapshot) =>
				{
					// Loop over the children
					NSEnumerator children = snapshot.Children;
					var child = children.NextObject() as DataSnapshot;

					List<Message> arrMessages = new List<Message>();
					while (child != null)
					{
						var dictionaryData = child.GetValue<NSDictionary>();
						Message message = Message.fromDictionary(dictionaryData);
						message.message_id = child.Key;
						message.roomId = roomId;
						arrMessages.Add(message);
						child = children.NextObject() as DataSnapshot;
					}
					messagesCallback(arrMessages);
				});

		}

	}
}

