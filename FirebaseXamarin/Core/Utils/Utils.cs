using System;
namespace FirebaseXamarin.Core.Utils
{
	public class Utils
	{
		public Utils()
		{

		}

		public static DateTime getFormmatedTime(string unixTime)
		{
			DateTimeOffset offset = DateTimeOffset.FromUnixTimeMilliseconds((long)Convert.ToUInt64(unixTime));
			return offset.LocalDateTime;
		}

		public static string getCurrentTime()
		{
			Int64 unixTimestamp = (Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
			return unixTimestamp + "";
		}
	}
}
