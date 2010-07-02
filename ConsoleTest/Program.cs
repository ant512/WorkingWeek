using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkingWeek;

namespace ConsoleTest
{
	class Program
	{
		static void Main(string[] args)
		{
			Week week = new Week();

			week.GetDay(DayOfWeek.Monday).AddShift(9, 30, 0, 0, new TimeSpan(2, 0, 0));
			week.GetDay(DayOfWeek.Monday).AddShift(12, 30, 0, 0, new TimeSpan(2, 0, 0));
			week.GetDay(DayOfWeek.Monday).AddShift(15, 30, 0, 0, new TimeSpan(2, 0, 0));

			week.GetDay(DayOfWeek.Tuesday).AddShift(9, 30, 0, 0, new TimeSpan(2, 0, 0));
			week.GetDay(DayOfWeek.Tuesday).AddShift(12, 30, 0, 0, new TimeSpan(2, 0, 0));
			week.GetDay(DayOfWeek.Tuesday).AddShift(15, 30, 0, 0, new TimeSpan(2, 0, 0));

			week.GetDay(DayOfWeek.Wednesday).AddShift(9, 30, 0, 0, new TimeSpan(2, 0, 0));
			week.GetDay(DayOfWeek.Wednesday).AddShift(12, 30, 0, 0, new TimeSpan(2, 0, 0));
			week.GetDay(DayOfWeek.Wednesday).AddShift(15, 30, 0, 0, new TimeSpan(2, 0, 0));

			week.GetDay(DayOfWeek.Tuesday).RemoveShift(12, 30, 0, 0);

			foreach (Shift shift in week.AscendingShifts(new DateTime(2010, 6, 28, 0, 0, 0), new DateTime(2010, 6, 30, 0, 0, 0)))
			{
				System.Diagnostics.Debug.WriteLine(shift.StartTime);
			}

			foreach (Shift shift in week.DescendingShifts(new DateTime(2010, 6, 30, 0, 0, 0), new DateTime(2010, 6, 28, 0, 0, 0)))
			{
				System.Diagnostics.Debug.WriteLine(shift.StartTime);
			}

			System.Diagnostics.Debug.WriteLine("");

			System.Diagnostics.Debug.WriteLine(week.DateAdd(new DateTime(2010, 6, 30, 0, 0, 0), new TimeSpan(29, 2, 3, 0)));

			System.Diagnostics.Debug.WriteLine("");
			System.Diagnostics.Debug.WriteLine(week.DateAdd(new DateTime(2010, 6, 30, 0, 0, 0), new TimeSpan(-29, 2, 0, 0)));

			System.Diagnostics.Debug.WriteLine("");
			System.Diagnostics.Debug.WriteLine(week.DateDiff(new DateTime(2010, 6, 30, 0, 0, 0), new DateTime(2010, 7, 1, 0, 0, 0)));

			System.Diagnostics.Debug.WriteLine("");
			System.Diagnostics.Debug.WriteLine(week.DateDiff(new DateTime(2010, 7, 1, 0, 0, 0), new DateTime(2010, 6, 30, 0, 0, 0)));

			System.Diagnostics.Debug.WriteLine("");
			System.Diagnostics.Debug.WriteLine(week.Duration);
		}
	}
}
