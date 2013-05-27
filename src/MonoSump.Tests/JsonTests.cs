using System;
using NUnit.Framework;
using System.Collections.Generic;
using Earlz.MonoSump.Core;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Earlz.MonoSump.Tests
{
	[TestFixture()]
	public class JsonTests
	{

		[Test]
		public void DataOutputIsValidJson()
		{
			var random=new Random(123);
			var list=new List<bool[]>();
			for(int i=0;i<10;i++)
			{
				list.Add(new bool[10]);
				for(int j=0;j<10;j++)
				{
					list[i][j]=random.Next(2) > 0;
				}
			}
			var json=JObject.Parse(list.ToJson());
			random=new Random(123);
			for(int i=0;i<10;i++)
			{
				var channel=json["channels"][i];
				for(int j=0;j<10;j++)
				{
					var expected=random.Next(2);
					Assert.AreEqual(expected, channel[j].ToObject<int>());
				}
			}
		}
		[Test]
		public void LooseConfigJsonWorks()
		{
			string s=@"
{
	foo: ""bar""
}";
			var json=JObject.Parse(s);
			Assert.AreEqual("bar", (string)json["foo"]);
			var obj=JsonConvert.DeserializeObject<TestObject>(s);
			Assert.AreEqual("bar", obj.foo);

		}
		[Test]
		public void ExampleConfigIsLoadedCorrectly()
		{
			string s=@"
stages: [ 
{
  triggers: { 1:1, 5:0 },
  start: true
},
{
  triggers: {0:1, 10:1, 12:0}
}],
delayCount: 50";
			var config=SumpConfiguration.LoadFromJson(s);
			Assert.AreEqual(10, config.ReadCount); //ensure readcount is left at default
			Assert.AreEqual(1, config.TriggerSetup[0].Values[1]);
			Assert.AreEqual(0, config.TriggerSetup[0].Values[5]);
			Assert.AreEqual(50, config.DelayCount);
			Assert.AreEqual(true, config.TriggerSetup[0].Start);
			Assert.AreEqual(1, config.TriggerSetup[1].Values[0]);
			Assert.AreEqual(1, config.TriggerSetup[1].Values[10]);
			Assert.AreEqual(0, config.TriggerSetup[1].Values[12]);
			Assert.AreEqual(false, config.TriggerSetup[0].Values.ContainsKey(3)); //ensure unset values are not inserted

		}
		public class TestObject
		{
			public string foo{get;set;}
		}



	}
}

