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
			Calendar calendar = new Calendar(new DayCollection());

			calendar.WorkingDays.GetDay(DayOfWeek.Monday).Shifts.Add(new Shift(9, 30, 0, 0, 1000000000));
			calendar.WorkingDays.GetDay(DayOfWeek.Monday).Shifts.Add(new Shift(12, 30, 0, 0, 1000000000));
			calendar.WorkingDays.GetDay(DayOfWeek.Monday).Shifts.Add(new Shift(15, 30, 0, 0, 1000000000));

			calendar.WorkingDays.GetDay(DayOfWeek.Tuesday).Shifts.Add(new Shift(9, 30, 0, 0, 1000000000));
			calendar.WorkingDays.GetDay(DayOfWeek.Tuesday).Shifts.Add(new Shift(12, 30, 0, 0, 1000000000));
			calendar.WorkingDays.GetDay(DayOfWeek.Tuesday).Shifts.Add(new Shift(15, 30, 0, 0, 1000000000));

			calendar.WorkingDays.GetDay(DayOfWeek.Wednesday).Shifts.Add(new Shift(9, 30, 0, 0, 1000000000));
			calendar.WorkingDays.GetDay(DayOfWeek.Wednesday).Shifts.Add(new Shift(12, 30, 0, 0, 1000000000));
			calendar.WorkingDays.GetDay(DayOfWeek.Wednesday).Shifts.Add(new Shift(15, 30, 0, 0, 1000000000));

			foreach (Shift shift in calendar.AscendingShifts(new DateTime(2010, 6, 28, 0, 0, 0), new DateTime(2010, 6, 30, 0, 0, 0)))
			{
				System.Diagnostics.Debug.WriteLine(shift.StartTime);
			}

			foreach (Shift shift in calendar.DescendingShifts(new DateTime(2010, 6, 30, 0, 0, 0), new DateTime(2010, 6, 28, 0, 0, 0)))
			{
				System.Diagnostics.Debug.WriteLine(shift.StartTime);
			}
		}
	}
}
