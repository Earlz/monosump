using System;

namespace Earlz.MonoSump
{

	public interface ISumpCommander
	{
		void Reset();
		void Run();
		int GetID();
		void SetTriggerMasks(bool[] mask);
		void SetTriggerValues(bool[] values);
		void SetTriggerConfigurations(TriggerConfiguration[] configs);


	}
}

