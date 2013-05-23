using System;
using System.IO.Ports;

namespace Earlz.MonoSump
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("MonoSump -- Sump Logic Analyzer Client");

			var handler=new ArgumentHandler();
			var config=handler.ParseCommands(args);
			using(var serial=new Serial(config.DeviceName, 115200))
			{
				var commander=new SumpCommander(serial);
				Console.WriteLine("Resetting device");
				commander.Reset();
				if(config.IdentifyOnly)
				{
					Console.WriteLine("Device ID: "+commander.GetID());
				}
			}
		}

	}
}
