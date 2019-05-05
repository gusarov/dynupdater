using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynUpdater
{
	class Logger
	{
		public static void WriteLine(string str)
		{
			Console.WriteLine(str);
			var tmp = Path.GetTempPath();
			File.AppendAllText(Path.Combine(tmp, "DynUpdater.log"), str + Environment.NewLine);
		}
	}
}
