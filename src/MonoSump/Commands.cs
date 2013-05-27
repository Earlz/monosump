using System;
using System.Collections.Generic;

namespace Earlz.MonoSump
{
	public class Commands
	{
		public bool Identify=false;
		public string DeviceName;
		public Dictionary<int, int> Triggers=new Dictionary<int, int>();
		public string DataOut;
		public bool JsonOut=false;
		public bool Verbose=false;
		public string ConfigFile;
		public int Samples=100;
		public int Frequency=1000;
	}
}

