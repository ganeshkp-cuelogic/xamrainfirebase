using System;
namespace FirebaseXamarin.Core.Network
{
	public class APIResult
	{
		public APIResult()
		{
		}

		/// <summary>
		/// The response json.
		/// </summary>
		private String responseJSON;
		public string ResponseJSON
		{
			get
			{
				return responseJSON;
			}

			set
			{
				responseJSON = value;
			}
		}


		/// <summary>
		/// The error.
		/// </summary>
		private GPError error;
		public GPError Error
		{
			get
			{
				return error;
			}

			set
			{
				error = value;
			}
		}


	}
}
