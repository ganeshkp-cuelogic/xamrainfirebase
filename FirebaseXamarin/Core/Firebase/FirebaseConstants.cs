using System;
namespace FirebaseXamarin
{
	public class FirebaseConstants
	{
		public FirebaseConstants()
		{
		}

		public static string FB_TOKEN = "FirebaseToken";

		public static string FB_USERS = "users";
		public static string FB_ROOMS = "rooms";
		public static string FB_NODE_MESSAGES = "messages";


		//RoomMeta
		public static string FB_ROOM_ID = "roomId";
		public static string FB_DISPLAY_NAME = "displayName";
		public static string FB_CREATED_TIME = "createdTime";
		public static string FB_CREATED_BY = "createdBy";
		public static string FB_LAST_UPDATED_TIME = "lastUpdatedTime";

		//Message
		public static string FB_MESSAGE = "message";
		public static string FB_SENDER_UID = "senderUid";
		public static string FB_TIMESTAMP = "timestamp";

	}
}
