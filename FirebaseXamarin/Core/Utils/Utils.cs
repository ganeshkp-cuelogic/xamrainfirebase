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
			DateTimeOffset offset = DateTimeOffset.FromUnixTimeMilliseconds((long)Convert.ToDouble(unixTime));
			return offset.DateTime;
		}

	}
}
