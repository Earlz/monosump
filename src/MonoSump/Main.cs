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

				//test it out

				var masks=new bool[32];
				masks[0]=true;
				commander.SetTriggerMasks(0, masks);
				var values=new bool[32];
				commander.SetTriggerValues(0, values);
				commander.SetTriggerConfigurations(0, new TriggerConfiguration(){Start=true, Level=0});
				commander.Run();
				var data=commander.GetData(1000, 10000);
				Console.WriteLine("done with "+data.Count+" frames");
				foreach(var d in data)
				{
					foreach(var b in d)
					{
						if(b)
						{
							Console.Write("1");
						}else
						{
							Console.Write("0");
						}
					}
					Console.WriteLine("");
				}
			}
		}

	}
}
