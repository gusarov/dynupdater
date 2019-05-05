using System;
using System.IO;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace DynUpdater
{
	partial class TheWindowsService : ServiceBase
	{
		public TheWindowsService()
		{
			InitializeComponent();
		}

		string[] _args;
		protected override void OnStart(string[] args)
		{
			// TODO: Add code here to start your service.
			StartImpl(args);
		}

		protected override void OnStop()
		{
			// TODO: Add code here to perform any tear-down necessary to stop your service.
			StopImpl();
		}

		Timer _timer;

		public void StartImpl(string[] args)
		{
			_args = args;

			_timer = new Timer(Worker, null, 1, 5 * 60 * 1000);
		}

		string lastIp;

		private void Worker(object state)
		{
			// get ip
			string ip;
			using(var webClient1 = new WebClient())
			{
				ip = webClient1.DownloadString("https://api.ipify.org/");
				if (ip == lastIp)
				{
					Console.WriteLine($"No update neede. IP = {ip}");
					return;
				}
			}

			var utf8 = new UTF8Encoding(false, false);
			var user = _args[1];
			var password = _args[2];
			var net = _args[3];
			var cred = user + ":" + password;
			cred = Convert.ToBase64String(utf8.GetBytes(cred));

			var webClient = new WebClient();
			webClient.Headers[HttpRequestHeader.Authorization] = " Basic " + cred;
			var data = webClient.DownloadString($"https://updates.opendns.com/nic/update?hostname={net}");
			Console.WriteLine(data);
			File.AppendAllText("C:\\Temp\\log.txt", DateTime.Now + " " + data);
			if (data.ToLowerInvariant().StartsWith("good "))
			{
				// remember IP
				Console.WriteLine("IP remembered: " + ip);
				lastIp = ip;
			}
		}

		public void StopImpl()
		{

		}
	}
}
