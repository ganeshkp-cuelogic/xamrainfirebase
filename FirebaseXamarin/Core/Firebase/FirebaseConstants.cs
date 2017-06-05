using System;
namespace FirebaseXamarin
{
	public class FirebaseConstants
	{
		public FirebaseConstants()
		{
		}

		public static readonly string FB_TOKEN = "firebaseToken";

		public static readonly string FB_USERS = "users";
		public static readonly string FB_ROOMS = "rooms";
		public static readonly string FB_NODE_MESSAGES = "messages";

		//Users
		public static readonly string FB_IND_ROOM_IDS = "indRooms";
		public static readonly string FB_GRP_ROOM_IDS = "grpRooms";

		//RoomMeta
		public static readonly string FB_ROOM_ID = "roomId";
		public static readonly string FB_ROOM_TYPE = "type";
		public static readonly string FB_DISPLAY_NAME = "displayName";
		public static readonly string FB_CREATED_TIME = "createdTime";
		public static readonly string FB_CREATED_BY = "createdBy";
		public static readonly string FB_LAST_UPDATED_TIME = "lastUpdatedTime";

		//Message
		public static readonly string FB_MESSAGE = "message";
		public static readonly string FB_SENDER_UID = "senderUid";
		public static readonly string FB_TIMESTAMP = "timestamp";

	}
}
