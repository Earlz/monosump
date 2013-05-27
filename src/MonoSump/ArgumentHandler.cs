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
--json        #output data to dataout in JSON
--verbose     #show extra info
--trigger N=V #Setup a stage-1 trigger at channel N for value V(1 or 0)
--samples N   #Get N number of samples
--frequency F #Set the sample frequency to F hz
--config file #load a configuration file (this will overwrite all command line options except for --verbose and --json

Loading a configuration file will cause all command lien options to be ignored,
except for --verbose and --json

Because of the complexity of SUMP, only a basic subset of features is exposed in the command line
To get multi-stage triggers and more advanced configuration options, you must use a config file
For an example configuration file see <TODO>
");


		}
	}
}

