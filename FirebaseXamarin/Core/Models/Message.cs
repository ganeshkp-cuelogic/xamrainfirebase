using System;
using Foundation;
namespace FirebaseXamarin
{
	public class Message
	{
		public Message()
		{
		}

		public string message { get; set; }
		public string message_id { get; set; }
		public string sender_id { get; set; }
		public string timestamp { get; set; }
		public string roomId { get; set; }


		public NSDictionary toDictionary()
		{
			object[] keys = { FirebaseConstants.FB_MESSAGE, FirebaseConstants.FB_SENDER_UID, FirebaseConstants.FB_TIMESTAMP, FirebaseConstants.FB_ROOM_ID };
			object[] values = { this.message, this.sender_id, Convert.ToDouble(this.timestamp), this.roomId };
			var data = NSMutableDictionary.FromObjectsAndKeys(values, keys, keys.Length);
			return data;
		}

		public static Message fromDictionary(NSDictionary messageDictionary)
		{
			Message message = new Message();
			message.message = messageDictionary[FirebaseConstants.FB_MESSAGE].ToString();
			message.timestamp = messageDictionary[FirebaseConstants.FB_TIMESTAMP].ToString();
			message.sender_id = messageDictionary[FirebaseConstants.FB_SENDER_UID].ToString();
			message.roomId = messageDictionary[FirebaseConstants.FB_ROOM_ID].ToString();
			return message;
		}

	}
}
