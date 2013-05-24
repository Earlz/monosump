using System;
using System.Collections.Generic;

namespace Earlz.MonoSump
{

	public interface ISumpCommander
	{
		void Reset();
		void Run();
		string GetID();
		void SetTriggerMasks(int stage, bool[] mask);
		void SetTriggerValues(int stage, bool[] values);
		void SetTriggerConfigurations(int stage, TriggerConfiguration configs);
		void SetDivider(int divider);
		void SetSampleSizeAndDelay(int size, int delay);
		void SetFlags(SumpFlags flags);
		IList<bool[]> GetData(int expectedSize, int timeout);


	}
}

