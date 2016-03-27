using System;
using NUnit.Framework;

namespace TestDrDiscovery
{
	[TestFixture()]
	public class TestDBConnector
	{

		[Test()]
		public void TestGetDBTimeDifference() 
		{
			System.DateTime now = System.DateTime.UtcNow;
			Assert.AreEqual(0, DBConnector.GetDBTimeDifference(now, now));
		}

		[Test()]
		public void TestConvertDateTimeToBinaryString() 
		{
			System.DateTime now = System.DateTime.UtcNow;
			Assert.IsInstanceOf(typeof(string), DBConnector.ConvertDateTimeToBinaryString(now));
		}

		[Test()]
		public void TestConvertBinaryStringToDateTime() 
		{
			string now_str = DBConnector.ConvertDateTimeToBinaryString(System.DateTime.UtcNow);
			Assert.IsInstanceOf(typeof(System.DateTime), DBConnector.ConvertBinaryStringToDateTime(now_str));
		}
	}
}