using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
	[TestClass]
	public class DateAddTests
	{
		/// <summary>
		/// Tests positive date add for a single shift.
		/// </summary>
		[TestMethod]
		public void PositiveSingleShift()
		{
			WorkingWeek.Week week = new WorkingWeek.Week();
			week.GetDay(DayOfWeek.Monday).AddShift(9, 30, 0, 0, TimeSpan.FromHours(3));
			week.GetDay(DayOfWeek.Monday).AddShift(13, 30, 0, 0, TimeSpan.FromHours(4));
			week.GetDay(DayOfWeek.Tuesday).AddShift(9, 30, 0, 0, TimeSpan.FromHours(3));
			week.GetDay(DayOfWeek.Wednesday).AddShift(9, 30, 0, 0, TimeSpan.FromHours(3));

			Assert.AreEqual(new DateTime(2010, 7, 5, 12, 30, 0), week.DateAdd(new DateTime(2010, 7, 5, 9, 30, 0), TimeSpan.FromHours(3)));
		}

		/// <summary>
		/// Tests positive date add for two whole shifts.
		/// </summary>
		[TestMethod]
		public void PositiveTwoShifts()
		{
			WorkingWeek.Week week = new WorkingWeek.Week();
			week.GetDay(DayOfWeek.Monday).AddShift(9, 30, 0, 0, TimeSpan.FromHours(3));
			week.GetDay(DayOfWeek.Monday).AddShift(13, 30, 0, 0, TimeSpan.FromHours(4));
			week.GetDay(DayOfWeek.Tuesday).AddShift(9, 30, 0, 0, TimeSpan.FromHours(3));
			week.GetDay(DayOfWeek.Wednesday).AddShift(9, 30, 0, 0, TimeSpan.FromHours(3));

			Assert.AreEqual(new DateTime(2010, 7, 5, 17, 30, 0), week.DateAdd(new DateTime(2010, 7, 5, 9, 30, 0), TimeSpan.FromHours(7)));
		}

		/// <summary>
		/// Tests positive date add for two shifts, the first of which is shortened.
		/// </summary>
		[TestMethod]
		public void PositiveShortStartShift()
		{
			WorkingWeek.Week week = new WorkingWeek.Week();
			week.GetDay(DayOfWeek.Monday).AddShift(9, 30, 0, 0, TimeSpan.FromHours(3));
			week.GetDay(DayOfWeek.Monday).AddShift(13, 30, 0, 0, TimeSpan.FromHours(4));
			week.GetDay(DayOfWeek.Tuesday).AddShift(9, 30, 0, 0, TimeSpan.FromHours(3));
			week.GetDay(DayOfWeek.Wednesday).AddShift(9, 30, 0, 0, TimeSpan.FromHours(3));

			Assert.AreEqual(new DateTime(2010, 7, 5, 17, 30, 0), week.DateAdd(new DateTime(2010, 7, 5, 10, 30, 0), new TimeSpan(6, 0, 0)));
		}

		/// <summary>
		/// Tests positive date add for three shifts, the first and last of which are shortened.
		/// </summary>
		[TestMethod]
		public void PositiveShortStartEndShift()
		{
			WorkingWeek.Week week = new WorkingWeek.Week();
			week.GetDay(DayOfWeek.Monday).AddShift(9, 30, 0, 0, TimeSpan.FromHours(3));
			week.GetDay(DayOfWeek.Monday).AddShift(13, 30, 0, 0, TimeSpan.FromHours(4));
			week.GetDay(DayOfWeek.Tuesday).AddShift(9, 30, 0, 0, TimeSpan.FromHours(3));
			week.GetDay(DayOfWeek.Wednesday).AddShift(9, 30, 0, 0, TimeSpan.FromHours(3));

			Assert.AreEqual(new DateTime(2010, 7, 5, 17, 0, 0), week.DateAdd(new DateTime(2010, 7, 5, 10, 30, 0), new TimeSpan(5, 30, 0)));
		}

		/// <summary>
		/// Tests positive date add for a period of 30 days.
		/// </summary>
		[TestMethod]
		public void Positive30Days()
		{
			WorkingWeek.Week week = new WorkingWeek.Week();
			week.GetDay(DayOfWeek.Monday).AddShift(9, 30, 0, 0, TimeSpan.FromHours(3));
			week.GetDay(DayOfWeek.Monday).AddShift(13, 30, 0, 0, TimeSpan.FromHours(3));
			week.GetDay(DayOfWeek.Tuesday).AddShift(9, 30, 0, 0, TimeSpan.FromHours(3));
			week.GetDay(DayOfWeek.Wednesday).AddShift(9, 30, 0, 0, TimeSpan.FromHours(3));

			// Allocate 30 days to a working week of 12 hours.  Will take 60 weeks, so compare with 60*7
			Assert.AreEqual(new DateTime(2010, 7, 5, 9, 30, 0).AddDays(60 * 7), week.DateAdd(new DateTime(2010, 7, 5, 9, 30, 0), new TimeSpan(30, 0, 0, 0)));
		}

		/// <summary>
		/// Tests negative date add for a single shift.
		/// </summary>
		[TestMethod]
		public void NegativeSingleShift()
		{
			WorkingWeek.Week week = new WorkingWeek.Week();
			week.GetDay(DayOfWeek.Monday).AddShift(9, 30, 0, 0, TimeSpan.FromHours(3));
			week.GetDay(DayOfWeek.Monday).AddShift(13, 30, 0, 0, TimeSpan.FromHours(4));
			week.GetDay(DayOfWeek.Tuesday).AddShift(9, 30, 0, 0, TimeSpan.FromHours(3));
			week.GetDay(DayOfWeek.Wednesday).AddShift(9, 30, 0, 0, TimeSpan.FromHours(3));

			Assert.AreEqual(new DateTime(2010, 7, 5, 9, 30, 0), week.DateAdd(new DateTime(2010, 7, 5, 12, 30, 0), TimeSpan.FromHours(-3)));
		}

		/// <summary>
		/// Tests negative date add for two whole shifts.
		/// </summary>
		[TestMethod]
		public void NegativeTwoShifts()
		{
			WorkingWeek.Week week = new WorkingWeek.Week();
			week.GetDay(DayOfWeek.Monday).AddShift(9, 30, 0, 0, TimeSpan.FromHours(3));
			week.GetDay(DayOfWeek.Monday).AddShift(13, 30, 0, 0, TimeSpan.FromHours(4));
			week.GetDay(DayOfWeek.Tuesday).AddShift(9, 30, 0, 0, TimeSpan.FromHours(3));
			week.GetDay(DayOfWeek.Wednesday).AddShift(9, 30, 0, 0, TimeSpan.FromHours(3));

			Assert.AreEqual(new DateTime(2010, 7, 5, 9, 30, 0), week.DateAdd(new DateTime(2010, 7, 5, 17, 30, 0), TimeSpan.FromHours(-7)));
		}

		/// <summary>
		/// Tests negative date add for two shifts, the last of which is shortened.
		/// </summary>
		[TestMethod]
		public void NegativeShortStartShift()
		{
			WorkingWeek.Week week = new WorkingWeek.Week();
			week.GetDay(DayOfWeek.Monday).AddShift(9, 30, 0, 0, TimeSpan.FromHours(3));
			week.GetDay(DayOfWeek.Monday).AddShift(13, 30, 0, 0, TimeSpan.FromHours(4));
			week.GetDay(DayOfWeek.Tuesday).AddShift(9, 30, 0, 0, TimeSpan.FromHours(3));
			week.GetDay(DayOfWeek.Wednesday).AddShift(9, 30, 0, 0, TimeSpan.FromHours(3));

			Assert.AreEqual(new DateTime(2010, 7, 5, 10, 30, 0), week.DateAdd(new DateTime(2010, 7, 5, 17, 30, 0), new TimeSpan(-6, 0, 0)));
		}

		/// <summary>
		/// Tests negative date add for three shifts, the first and last of which are shortened.
		/// </summary>
		[TestMethod]
		public void NegativeShortStartEndShift()
		{
			WorkingWeek.Week week = new WorkingWeek.Week();
			week.GetDay(DayOfWeek.Monday).AddShift(9, 30, 0, 0, TimeSpan.FromHours(3));
			week.GetDay(DayOfWeek.Monday).AddShift(13, 30, 0, 0, TimeSpan.FromHours(4));
			week.GetDay(DayOfWeek.Tuesday).AddShift(9, 30, 0, 0, TimeSpan.FromHours(3));
			week.GetDay(DayOfWeek.Wednesday).AddShift(9, 30, 0, 0, TimeSpan.FromHours(3));

			Assert.AreEqual(new DateTime(2010, 7, 5, 10, 30, 0), week.DateAdd(new DateTime(2010, 7, 5, 17, 0, 0), new TimeSpan(-5, -30, 0)));
		}


		/// <summary>
		/// Tests negative date add for a period of 30 days.
		/// </summary>
		[TestMethod]
		public void Negative30Days()
		{
			WorkingWeek.Week week = new WorkingWeek.Week();
			week.GetDay(DayOfWeek.Monday).AddShift(9, 30, 0, 0, TimeSpan.FromHours(3));
			week.GetDay(DayOfWeek.Monday).AddShift(13, 30, 0, 0, TimeSpan.FromHours(3));
			week.GetDay(DayOfWeek.Tuesday).AddShift(9, 30, 0, 0, TimeSpan.FromHours(3));
			week.GetDay(DayOfWeek.Wednesday).AddShift(9, 30, 0, 0, TimeSpan.FromHours(3));

			// Allocate 30 days to a working week of 12 hours.  Will take 60 weeks, so compare with 60*7
			Assert.AreEqual(new DateTime(2010, 7, 5, 9, 30, 0), week.DateAdd(new DateTime(2010, 7, 5, 9, 30, 0).AddDays(60 * 7), new TimeSpan(-30, 0, 0, 0)));
		}
	}
}
