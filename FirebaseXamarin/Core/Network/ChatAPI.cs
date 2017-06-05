using System;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace FirebaseXamarin.Core.Network
{
	public class ChatAPI
	{

		private static string URL_BATCH_ADD_TO_TOPIC = "https://iid.googleapis.com/iid/v1:batchAdd";
		private static string URL_SEND_MESSAGE = "https://fcm.googleapis.com/fcm/send";

		public ChatAPI()
		{
		}

		public static async void subscribeOtherUsersToTopic(RoomsMetaData roomMetaData)
		{
			SubscribeTopic subscribeTopic = new SubscribeTopic();
			subscribeTopic.to = "/topics/" + roomMetaData.roomId;
			subscribeTopic.registration_tokens = roomMetaData.users;
			APIResult result = await NetworkRequestManager.Sharedmanager.sendPostRequest(JsonConvert.SerializeObject(subscribeTopic), URL_BATCH_ADD_TO_TOPIC);
		}

		public static async void sendMessage(Message message)
		{
			MessageFormat format = new MessageFormat();
			format.to = "/topics/" + message.roomId;
			format.priority = "high";

			Notification notification = new Notification();
			notification.body = message.message;
			notification.title = message.message;
			notification.text = message.message;
			notification.uid = message.sender_id;
			notification.timeStamp = message.timestamp;

			format.notification = notification;

			APIResult result = await NetworkRequestManager.Sharedmanager.sendPostRequest(JsonConvert.SerializeObject(format), URL_SEND_MESSAGE);
		}
	}

	public class SubscribeTopic
	{
		public string to { get; set; }
		public List<string> registration_tokens { get; set; }
	}

	public class MessageFormat
	{
		public string to { get; set; }
		public string priority { get; set; }
		public Notification notification { get; set; }
	}

	public class Notification
	{
		public string title { get; set; }
		public string text { get; set; }
		public string uid { get; set; }
		public string body { get; set; }
		public string timeStamp { get; set; }
	}

}
