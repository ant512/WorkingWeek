using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkingCalendar;

namespace ConsoleTest
{
	class Program
	{
		static void Main(string[] args)
		{
			Calendar calendar = new Calendar(new Week());

			calendar.Week.GetDay(DayOfWeek.Monday).AddShift(9, 30, 0, 0, new TimeSpan(2, 0, 0));
			calendar.Week.GetDay(DayOfWeek.Monday).AddShift(12, 30, 0, 0, new TimeSpan(2, 0, 0));
			calendar.Week.GetDay(DayOfWeek.Monday).AddShift(15, 30, 0, 0, new TimeSpan(2, 0, 0));

			calendar.Week.GetDay(DayOfWeek.Tuesday).AddShift(9, 30, 0, 0, new TimeSpan(2, 0, 0));
			calendar.Week.GetDay(DayOfWeek.Tuesday).AddShift(12, 30, 0, 0, new TimeSpan(2, 0, 0));
			calendar.Week.GetDay(DayOfWeek.Tuesday).AddShift(15, 30, 0, 0, new TimeSpan(2, 0, 0));

			calendar.Week.GetDay(DayOfWeek.Wednesday).AddShift(9, 30, 0, 0, new TimeSpan(2, 0, 0));
			calendar.Week.GetDay(DayOfWeek.Wednesday).AddShift(12, 30, 0, 0, new TimeSpan(2, 0, 0));
			calendar.Week.GetDay(DayOfWeek.Wednesday).AddShift(15, 30, 0, 0, new TimeSpan(2, 0, 0));

			calendar.Week.GetDay(DayOfWeek.Tuesday).RemoveShift(12, 30, 0, 0);

			foreach (Shift shift in calendar.AscendingShifts(new DateTime(2010, 6, 28, 0, 0, 0), new DateTime(2010, 6, 30, 0, 0, 0)))
			{
				System.Diagnostics.Debug.WriteLine(shift.StartTime);
			}

			foreach (Shift shift in calendar.DescendingShifts(new DateTime(2010, 6, 30, 0, 0, 0), new DateTime(2010, 6, 28, 0, 0, 0)))
			{
				System.Diagnostics.Debug.WriteLine(shift.StartTime);
			}

			System.Diagnostics.Debug.WriteLine("");

			System.Diagnostics.Debug.WriteLine(calendar.DateAdd(new DateTime(2010, 6, 30, 0, 0, 0), new TimeSpan(29, 2, 3, 0)));

			System.Diagnostics.Debug.WriteLine("");
			System.Diagnostics.Debug.WriteLine(calendar.DateAdd(new DateTime(2010, 6, 30, 0, 0, 0), new TimeSpan(-29, 2, 0, 0)));

			System.Diagnostics.Debug.WriteLine("");
			System.Diagnostics.Debug.WriteLine(calendar.DateDiff(new DateTime(2010, 6, 30, 0, 0, 0), new DateTime(2010, 7, 1, 0, 0, 0)));

			System.Diagnostics.Debug.WriteLine("");
			System.Diagnostics.Debug.WriteLine(calendar.DateDiff(new DateTime(2010, 7, 1, 0, 0, 0), new DateTime(2010, 6, 30, 0, 0, 0)));

			System.Diagnostics.Debug.WriteLine("");
			System.Diagnostics.Debug.WriteLine(calendar.Week.Duration);
		}
	}
}
