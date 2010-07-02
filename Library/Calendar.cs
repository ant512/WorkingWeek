using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkingCalendar
{
	public class Calendar
	{
		#region Properties

		public DayCollection WorkingDays
		{
			get;
			set;
		}

		public bool ContainsShifts
		{
			get
			{
				for (int i = 0; i < 7; ++i)
				{
					if (WorkingDays.GetDay((DayOfWeek)i).IsWorking) return true;
				}

				return false;
			}
		}

		#endregion

		#region Constructors

		public Calendar(DayCollection days)
		{
			WorkingDays = days;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Check if the given date/time falls within a shift.
		/// </summary>
		/// <param name="date">The date to check.</param>
		/// <returns>True if the date falls within a shift.</returns>
		public bool IsWorkingDate(DateTime date)
		{
			Day workingDay = WorkingDays.GetDay(date.DayOfWeek);

			if (workingDay == null) return false;

			if (!workingDay.ContainsShifts) return false;

			return workingDay.IsWorkingTime(date);
		}

		/// <summary>
		/// Custom iterator that produces each shift between the two dates.
		/// </summary>
		/// <param name="startDate"></param>
		/// <param name="endDate"></param>
		/// <returns></returns>
		public IEnumerable<Shift> AscendingShifts(DateTime startDate, DateTime endDate)
		{
			if (ContainsShifts)
			{

				// Start with the day appropriate to the supplied ate
				Day day = WorkingDays.GetDay(startDate.DayOfWeek);
				DateTime currentDate = startDate;

				// Continue looping until we hit the end date
				while (currentDate < endDate)
				{
					Shift shift = null;

					// Loop until we find a valid shift
					while (shift == null)
					{
						shift = day.GetNextShift(currentDate);

						// Move to the next day if there are no matches on this day
						if (shift == null)
						{
							currentDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day);
							currentDate = currentDate.AddDays(1);
							day = WorkingDays.GetDay(currentDate.DayOfWeek);
						}
					}

					if (shift != null)
					{
						// Ensure that the shift contains the correct date
						Shift adjustedShift = new Shift(new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, shift.StartTime.Hour, shift.StartTime.Minute, shift.StartTime.Millisecond), shift.Duration);
						currentDate = adjustedShift.EndTime;

						// Return the adjusted shift
						yield return adjustedShift;
					}
				}
			}
		}

		public IEnumerable<Shift> DescendingShifts(DateTime startDate, DateTime endDate)
		{
			if (ContainsShifts)
			{

				// Start with the day appropriate to the supplied ate
				Day day = WorkingDays.GetDay(startDate.DayOfWeek);
				DateTime currentDate = startDate;

				// Continue looping until we hit the end date
				while (currentDate > endDate)
				{
					Shift shift = null;

					// Loop until we find a valid shift
					while ((shift == null) && (currentDate > endDate))
					{
						shift = day.GetPreviousShift(currentDate);

						// Move to the previous day if there are no matches on this day
						if (shift == null)
						{
							currentDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 23, 59, 59, 999);
							currentDate = currentDate.AddDays(-1);
							day = WorkingDays.GetDay(currentDate.DayOfWeek);
						}
					}

					if (shift != null)
					{
						// Ensure that the shift contains the correct date
						Shift adjustedShift = new Shift(new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, shift.StartTime.Hour, shift.StartTime.Minute, shift.StartTime.Millisecond), shift.Duration);
						currentDate = adjustedShift.StartTime.AddMilliseconds(-1);

						// Return the adjusted shift
						yield return adjustedShift;
					}
				}
			}
		}

		#endregion
	}
}
