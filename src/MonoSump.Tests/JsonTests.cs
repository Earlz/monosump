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
			Assert.AreEqual("bar", json["foo"].ToString());
			var obj=JsonConvert.DeserializeObject<TestObject>(s);
			Assert.AreEqual("bar", obj.foo);

		}
		public class TestObject
		{
			public string foo{get;set;}
		}



	}
}

