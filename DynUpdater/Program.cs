using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DynUpdater
{
	class Program
	{
		static void Main(string[] args)
		{
			var utf8 = new UTF8Encoding(false, false);
			var user = args[0];
			var password = args[1];
			var net = args[2];
			var cred = user + ":" + password;
			cred = Convert.ToBase64String(utf8.GetBytes(cred));

			var webClient = new WebClient();
			webClient.Headers[HttpRequestHeader.Authorization] = " Basic " + cred;
			var data = webClient.DownloadString($"https://updates.opendns.com/nic/update?hostname={net}");
			Console.WriteLine(data);
		}
	}
}
