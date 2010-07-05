using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
	[TestClass]
	public class DateDiffTests
	{
		/// <summary>
		/// Tests positive date diff for whole shifts.
		/// </summary>
		[TestMethod]
		public void TestDateDiff1()
		{
			WorkingWeek.Week week = new WorkingWeek.Week();
			week.GetDay(DayOfWeek.Monday).AddShift(9, 30, 0, 0, TimeSpan.FromHours(3));
			week.GetDay(DayOfWeek.Monday).AddShift(13, 30, 0, 0, TimeSpan.FromHours(4));
			week.GetDay(DayOfWeek.Tuesday).AddShift(9, 30, 0, 0, TimeSpan.FromHours(3));
			week.GetDay(DayOfWeek.Wednesday).AddShift(9, 30, 0, 0, TimeSpan.FromHours(3));

			Assert.AreEqual(TimeSpan.FromHours(13), week.DateDiff(new DateTime(2010, 7, 5, 9, 30, 0), new DateTime(2010, 7, 9, 12, 30, 0)));
		}

		/// <summary>
		/// Tests positive date diff for partial shifts.
		/// </summary>
		[TestMethod]
		public void TestDateDiff2()
		{
			WorkingWeek.Week week = new WorkingWeek.Week();
			week.GetDay(DayOfWeek.Monday).AddShift(9, 30, 0, 0, TimeSpan.FromHours(3));
			week.GetDay(DayOfWeek.Monday).AddShift(13, 30, 0, 0, TimeSpan.FromHours(4));
			week.GetDay(DayOfWeek.Tuesday).AddShift(9, 30, 0, 0, TimeSpan.FromHours(3));
			week.GetDay(DayOfWeek.Wednesday).AddShift(9, 30, 0, 0, TimeSpan.FromHours(3));

			Assert.AreEqual(new TimeSpan(0, 12, 30, 0, 0), week.DateDiff(new DateTime(2010, 7, 5, 10, 0, 0), new DateTime(2010, 7, 9, 12, 30, 0)));
		}

		/// <summary>
		/// Tests negative date diff for whole shifts.
		/// </summary>
		[TestMethod]
		public void TestDateDiff3()
		{
			WorkingWeek.Week week = new WorkingWeek.Week();
			week.GetDay(DayOfWeek.Monday).AddShift(9, 30, 0, 0, TimeSpan.FromHours(3));
			week.GetDay(DayOfWeek.Monday).AddShift(13, 30, 0, 0, TimeSpan.FromHours(4));
			week.GetDay(DayOfWeek.Tuesday).AddShift(9, 30, 0, 0, TimeSpan.FromHours(3));
			week.GetDay(DayOfWeek.Wednesday).AddShift(9, 30, 0, 0, TimeSpan.FromHours(3));

			Assert.AreEqual(TimeSpan.FromHours(-13), week.DateDiff(new DateTime(2010, 7, 9, 12, 30, 0), new DateTime(2010, 7, 5, 9, 30, 0)));
		}

		/// <summary>
		/// Tests negative date diff for partial shifts.
		/// </summary>
		[TestMethod]
		public void TestDateDiff4()
		{
			WorkingWeek.Week week = new WorkingWeek.Week();
			week.GetDay(DayOfWeek.Monday).AddShift(9, 30, 0, 0, TimeSpan.FromHours(3));
			week.GetDay(DayOfWeek.Monday).AddShift(13, 30, 0, 0, TimeSpan.FromHours(4));
			week.GetDay(DayOfWeek.Tuesday).AddShift(9, 30, 0, 0, TimeSpan.FromHours(3));
			week.GetDay(DayOfWeek.Wednesday).AddShift(9, 30, 0, 0, TimeSpan.FromHours(3));

			Assert.AreEqual(new TimeSpan(0, -12, -30, 0, 0), week.DateDiff(new DateTime(2010, 7, 9, 12, 30, 0), new DateTime(2010, 7, 5, 10, 0, 0)));
		}
	}
}
