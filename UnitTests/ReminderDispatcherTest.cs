using System;
using System.Threading;
using Microsoft.Office.Interop.Outlook;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperReminder;
using SuperReminder.Models;

namespace UnitTests
{
	[TestClass]
	public class ReminderDispatcherTest
	{
		[TestMethod]
		public void ApplicationReminderTest()
		{
			var target = new ReminderDispatcher();

			target.DispatchReminder(new ReminderInfo
			{
				Subject = "Some Meeting",
				Location = "CH7-311",
				Body = "Body",
				Duration = TimeSpan.FromHours(1),
				StartTime = DateTime.Now + TimeSpan.FromMinutes(10)
			});

			Thread.Sleep(1000);

			target.DispatchReminder(new ReminderInfo
			{
				Subject = "Some Other Meeting",
				Location = "CH7-311",
				Body = "Body",
				Duration = TimeSpan.FromHours(1),
				StartTime = DateTime.Now + TimeSpan.FromMinutes(10)
			});

			target.Wait();
		}
	}
}