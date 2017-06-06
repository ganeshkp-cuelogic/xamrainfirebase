using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FirebaseXamarin.Core.Utils;

namespace FirebaseXamarin.Core.Network
{
	public class NetworkRequestManager
	{

		private HttpClient client;
		private static NetworkRequestManager sharedmanager = new NetworkRequestManager();
		public static NetworkRequestManager Sharedmanager
		{
			get
			{
				return sharedmanager;
			}
		}

		public NetworkRequestManager()
		{
			client = new HttpClient();
			client.MaxResponseContentBufferSize = 2560000;
			string header = "key=AIzaSyBTLdwMLey8d1YkEd3rSLEWsYxAwEYuBTM";
			client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", header);
		}

		#region Protocol Methods
		public async Task<APIResult> sendPostRequest(String inputJson, String url)
		{
			var uri = new Uri(url);
			var content = new StringContent(inputJson, Encoding.UTF8, "application/json");

			Console.WriteLine("Url - " + url + " - \n" + inputJson);

			HttpResponseMessage response = null;
			response = await client.PostAsync(uri, content);
			var contentBody = await response.Content.ReadAsStringAsync();

			APIResult apiResult = new APIResult();
			if (response.IsSuccessStatusCode)
			{
				apiResult.ResponseJSON = contentBody;
			}
			else
			{
				apiResult.Error = new GPError(CCMessages.SOMETHING_WENT_WRONG);
			}

			return apiResult;
		}

		public async Task<APIResult> sendGetRequest(String url)
		{
			var uri = new Uri(url);

			HttpResponseMessage response = null;
			client = new HttpClient();
			client.MaxResponseContentBufferSize = 2560000;
			client.DefaultRequestHeaders.Add("user-key", "64d1b8c4c768ac50faa034a333f6e9d1");
			response = await client.GetAsync(uri);
			var contentBody = await response.Content.ReadAsStringAsync();

			APIResult apiResult = new APIResult();
			if (response.IsSuccessStatusCode)
			{
				apiResult.ResponseJSON = contentBody;
			}
			else
			{
				apiResult.Error = new GPError(CCMessages.SOMETHING_WENT_WRONG);
			}
			return apiResult;
		}
		#endregion
	}


	public class ErrorResponse
	{
		public bool error;
		public string message;
	}
}

