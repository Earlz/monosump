using System;
using System.IO.Ports;
using Earlz.MonoSump.Core;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;

namespace Earlz.MonoSump
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("MonoSump -- Sump Logic Analyzer Client");

			var handler=new ArgumentHandler();
			var commands=handler.ParseCommands(args);
			if(commands==null)
			{
				return;
			}
			var config=new SumpConfiguration();
			if(commands.ConfigFile!=null)
			{
				var json=LoadFile(commands.ConfigFile);
				config=SumpConfiguration.LoadFromJson(json);
			}
			else
			{
				//apply command options
				config.DelayCount=config.ReadCount=commands.Samples;
				config.SampleFrequency=commands.Frequency;
				config.TriggerSetup[0]=new TriggerStageConfiguration();
				foreach(var kv in commands.Triggers)
				{
					config.TriggerSetup[0].Values.Add(kv);
				}
				config.TriggerSetup[0].Start=true;
			}

			using(var serial=new Serial(commands.DeviceName, 115200))
			{
				var sump=new SumpController(new SumpCommander(serial), commands.Verbose);
				if(commands.Identify)
				{
					sump.Sump.Reset();
					Console.WriteLine(sump.Sump.GetID());
				}
				var data=sump.Execute(config);

				Console.WriteLine("# done with "+data.Count+" frames");
				var sb=new StringBuilder();
				foreach(var d in data)
				{
					foreach(var b in d)
					{
						if(b)
						{
							sb.Append("1");
						}else
						{
							sb.Append("0");
						}
					}
					sb.AppendLine("");
				}
				if(commands.DataOut!=null)
				{
					if(commands.JsonOut)
					{
						WriteFile(commands.DataOut, data.ToJson());
	          		}
					else
					{
						WriteFile(commands.DataOut, sb.ToString());
					}
				}
				Console.WriteLine(sb.ToString());
			}
		}
		public static string LoadFile(string name)
		{
			using(var f=File.Open(name, FileMode.Open))
			{
				var reader=new StreamReader(f);
				return reader.ReadToEnd();
			}
		}
		public static void WriteFile(string name, string content)
		{
			using(var f=File.Open(name, FileMode.CreateNew))
			{
				var writer=new StreamWriter(f);
				writer.Write(content);
			}
		}

	}
}
