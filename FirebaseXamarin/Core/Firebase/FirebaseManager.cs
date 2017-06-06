using System;
using Firebase.Database;
using System.Collections.Generic;
using Foundation;
using Newtonsoft.Json;
using FirebaseXamarin.Core.Utils;
using System.Collections.Specialized;
using System.Linq;
using Firebase.CloudMessaging;
using FirebaseXamarin.Core.Network;

namespace FirebaseXamarin
{
	public class FirebaseManager
	{
		public FirebaseManager() { }

		public static FirebaseManager sharedManager = new FirebaseManager();

		private readonly DatabaseReference rootNode = Database.DefaultInstance.GetRootReference();

		public void syncNodes()
		{
			rootNode.GetChild(FirebaseConstants.FB_USERS).KeepSynced(true);
			rootNode.GetChild(FirebaseConstants.FB_ROOMS).KeepSynced(true);
			rootNode.GetChild(FirebaseConstants.FB_NODE_MESSAGES).KeepSynced(true);
		}

		public void updateUserInfo(User user)
		{
			DatabaseReference userNode = rootNode.GetChild("users").GetChild(user.uid);
			userNode.UpdateChildValues(user.ToDictionary());
		}

		public void getAllUser(Action<List<User>> callback)
		{
			DatabaseReference userNode = rootNode.GetChild(FirebaseConstants.FB_USERS);
			userNode.KeepSynced(true);
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

		public void createGroup(RoomsMetaData roomMetatData, Action<string> roomIdCallBack)
		{
			string roomIDKey;
			DatabaseReference roomNode = rootNode.GetChild(FirebaseConstants.FB_ROOMS);

			if (roomMetatData.type == Constants.ROOM_TYPE_GROUP)
			{
				roomIDKey = roomNode.GetChildByAutoId().Key;
				roomMetatData.roomId = roomIDKey;
				updateTheRoomInfo(roomMetatData);
				roomNode.GetChild(roomIDKey).UpdateChildValues(roomMetatData.toDictionary());
				roomIdCallBack(roomIDKey);

				subscribeToTopic(roomMetatData);
			}
			else
			{
				/**
                 * Check if a room exists or not between us
                 * 
                 */
				AlreadyRoomExists(roomMetatData, (string roomId) =>
				{
					roomMetatData.roomId = roomId;
					if (String.IsNullOrEmpty(roomMetatData.roomId))
					{
						roomIDKey = roomNode.GetChildByAutoId().Key;
						roomMetatData.roomId = roomIDKey;

						updateTheRoomInfo(roomMetatData);
					}
					else
					{
						roomIDKey = roomMetatData.roomId;
					}
					roomNode.GetChild(roomIDKey).UpdateChildValues(roomMetatData.toDictionary());
					roomIdCallBack(roomIDKey);

					subscribeToTopic(roomMetatData);
				});
			}
		}

		private void AlreadyRoomExists(RoomsMetaData roomMetaData, Action<string> roomIdCallback)
		{
			User myInfo = DBManager.sharedManager.getLoggedInUserInfo();
			getUserInfo(myInfo.uid, (myUserInfo) =>
			{
				List<string> myIndRoomIds = myUserInfo.arrIndRoomId;

				List<string> otherUserIndRoomIds = new List<string>();
				string otherUserId = roomMetaData.users.Find(x => x != myUserInfo.uid);

				getUserInfo(otherUserId, (userInfo) =>
				{
					otherUserIndRoomIds = userInfo.arrIndRoomId;
					if (myIndRoomIds != null && myIndRoomIds.Count > 0 && otherUserIndRoomIds != null && otherUserIndRoomIds.Count > 0)
					{
						var commonIds = myIndRoomIds.Intersect(otherUserIndRoomIds);
						foreach (string value in commonIds)
						{
							Console.WriteLine(value);
							roomIdCallback(value);
							return;
						}
					}

					roomIdCallback(null);
				});
			});
		}

		private void updateTheRoomInfo(RoomsMetaData roomMeataData)
		{
			/**
			 * 1. Add the new Room ID in sender user info
			 * 2. Add the new Room ID in receiver user info 
			 */
			foreach (string userID in roomMeataData.users)
			{
				getUserInfo(userID, (User userInfo) =>
				{
					User roomUserInfo = userInfo;
					if (roomMeataData.type == Constants.ROOM_TYPE_ONE_ONE)
					{
						roomUserInfo.arrIndRoomId.Add(roomMeataData.roomId);
					}
					else
					{
						roomUserInfo.arrGroupRoomId.Add(roomMeataData.roomId);
					}
					updateUserInfo(roomUserInfo);
				});
			}
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
			int counter = 0;
			List<RoomsMetaData> roomMetaDataList = new List<RoomsMetaData>();
			getUserInfo(myUserId, (User userInfo) =>
			{
				if (userInfo != null)
				{
					//TODO- Change the logic of fetching the details from array of ids
					List<string> totalRoomsIDs = new List<string>();
					if (userInfo.arrIndRoomId != null)
					{
						totalRoomsIDs.AddRange(userInfo.arrIndRoomId);
					}

					if (userInfo.arrGroupRoomId != null)
					{
						totalRoomsIDs.AddRange(userInfo.arrGroupRoomId);
					}

					if (totalRoomsIDs.Count > 0)
					{
						counter = 0;
						foreach (string roomID in totalRoomsIDs)
						{
							getRoomInfo(roomID, (RoomsMetaData roomInfo) =>
							{
								if (roomInfo != null)
								{
									roomMetaDataList.Add(roomInfo);
									counter = counter + 1;
									roomsCallback(roomMetaDataList);
								}
							});
						}
					}
					else
					{
						roomsCallback(null);
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
					if (snapshot != null && snapshot.ChildrenCount > 0)
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
			usersNode.KeepSynced(true);
			DatabaseReference userDetailsNode = usersNode.GetChild(userId);
			userDetailsNode.ObserveSingleEvent(DataEventType.Value, (snapshot) =>
				{
					if (snapshot != null && snapshot.HasChildren)
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

		public void sendMessage(Message message)
		{
			DatabaseReference messageNode = rootNode.GetChild(FirebaseConstants.FB_NODE_MESSAGES);
			DatabaseReference roomNode = messageNode.GetChild(message.roomId);
			roomNode.GetChildByAutoId().UpdateChildValues(message.toDictionary());
		}

		public void subscribeToTopic(RoomsMetaData roomMeataData)
		{
			Messaging.SharedInstance.Subscribe("/topics/" + roomMeataData.roomId);
			Console.WriteLine("Subscribed to topic - /topics/" + roomMeataData.roomId);

			/**
			 * Subscribe other members of this group into this topic
			 */
			ChatAPI.subscribeOtherUsersToTopic(roomMeataData);
		}

		public void broadcastTopic(RoomsMetaData roomMeataData, Message message)
		{
			ChatAPI.sendMessage(roomMeataData, message);
		}

		public void unSubscribeToTopic(string topicName)
		{
			Messaging.SharedInstance.Unsubscribe("/topics/" + topicName);
		}
	}
}

