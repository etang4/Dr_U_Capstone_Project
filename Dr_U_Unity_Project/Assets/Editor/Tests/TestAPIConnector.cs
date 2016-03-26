using System;
using NUnit.Framework;

namespace TestDrDiscovery
{
	[TestFixture()]
	public class TestAPIConnector
	{
		[Test()]
		public void TestCheckNetworkStatus()
		{
			Assert.IsNotNull(APIConnector.CheckNetworkStatus());
		}

		[Test()]
		public void TestGetEstimotes()
		{
			Assert.IsNotEmpty(APIConnector.GetEstimotes());
		}

		[Test()]
		public void TestGetQuestions()
		{
			Assert.IsNotEmpty(APIConnector.GetQuestions());
		}

		[Test()]
		public void TestGetAnswers()
		{
			Assert.IsNotEmpty(APIConnector.GetAnswers());
		}

		[Test()]
		public void TestGetExhibits()
		{
			Assert.IsNotEmpty(APIConnector.GetExhibits());
		}
	}
}