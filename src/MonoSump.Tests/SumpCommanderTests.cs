using System;
using NUnit.Framework;
using Earlz.MonoSump.Core;

namespace Earlz.MonoSump.Tests
{
	[TestFixture()]
	public class SumpCommanderTests
	{

		[Test]
		public void IdentifyReturnsAscii()
		{
			var port=new TestPort();
			port.BytesToSend.AddRange(new byte[]{0x31, 0x41, 0x4C, 0x53});
			var sump=new SumpCommander(port);
			Assert.AreEqual("SLA1", sump.GetID());
			Assert.AreEqual(0x02, port.ReceivedBytes[0]);
		}
	}
}

