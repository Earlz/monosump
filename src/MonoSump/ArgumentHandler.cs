using System;

namespace Earlz.MonoSump
{
	public class ArgumentHandler
	{
		public ArgumentHandler()
		{
		}
		public Config ParseCommands(string[] args)
		{
			var config=new Config();
			foreach(var arg in args)
			{
				switch(arg)
				{
				case "--help":
					PrintHelp();
					return null;
				case "--identify":
					config.IdentifyOnly=true;
					break;
				default:
					if(config.DeviceName==null)
					{
						config.DeviceName=arg;
					}
					else
					{
						throw new ApplicationException("unexpected extra argument after devicename");
					}
					break;
				}
			}
			return config;
		}
		public static void PrintHelp()
		{
			Console.WriteLine(
				@"monosump is a SUMP logic analyzer client for .Net
usage: monosump [options] devicename [dataoutput]

Options:

--help        #this screen
--identify    #get the device ID
--json        #output data  to dataout in JSON
--verbose     #show extra info
--triggerSTAGE:N=V  
 #stage is 0-3 stage number of trigger
 #N is channel number
 #V is value(1 or 0) the trigger must have)
--triggerconfigSTAGE:");


		}
	}
}

