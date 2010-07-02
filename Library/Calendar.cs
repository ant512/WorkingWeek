using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkingCalendar
{
	public sealed class Calendar
	{
		#region Properties

		public Week Week
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
					if (Week.GetDay((DayOfWeek)i).IsWorking) return true;
				}

				return false;
			}
		}

		#endregion

		#region Constructors

		public Calendar(Week days)
		{
			Week = days;
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
			Day workingDay = Week.GetDay(date.DayOfWeek);

			if (workingDay == null) return false;

			if (!workingDay.ContainsShifts) return false;

			return workingDay.IsWorkingTime(date);
		}

		/// <summary>
		/// Custom iterator that produces each shift between the two dates.  Enter Date.MaxValue as
		/// the end date to produce an endless list of shifts.
		/// </summary>
		/// <param name="startDate"></param>
		/// <param name="endDate"></param>
		/// <returns></returns>
		public IEnumerable<Shift> AscendingShifts(DateTime startDate, DateTime endDate)
		{
			if (ContainsShifts)
			{
				// Start with the day appropriate to the supplied ate
				Day day = Week.GetDay(startDate.DayOfWeek);
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
							day = Week.GetDay(currentDate.DayOfWeek);
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
				Day day = Week.GetDay(startDate.DayOfWeek);
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
							day = Week.GetDay(currentDate.DayOfWeek);
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

		private DateTime DateAddPositive(DateTime startDate, TimeSpan duration)
		{
			DateTime endDate = startDate;

			// Calculate how many weeks we can allocate simultaneously to avoid
			// iterating over days
			long weeks = duration.Ticks / Week.Duration.Ticks;

			if (weeks > 0)
			{
				duration -= TimeSpan.FromTicks(weeks * Week.Duration.Ticks);
				endDate = endDate.AddDays(weeks * 7);
			}

			// Allocate remaining fraction of a week
			foreach (Shift shift in AscendingShifts(endDate, DateTime.MaxValue))
			{
				// Stop if we've allocated the entire duration
				if (duration.Ticks == 0) break;

				if (duration >= shift.Duration)
				{
					// Move the end date to the end of the shift, and subtract the length of the
					// shift from the remaining duration
					endDate = shift.EndTime;
					duration -= shift.Duration;
				}
				else
				{
					// Remaining duration is shorter than the shift
					endDate = shift.StartTime.AddTicks(duration.Ticks);
					duration -= duration;
				}
			}

			return endDate;
		}

		public DateTime DateAddNegative(DateTime startDate, TimeSpan duration)
		{
			DateTime endDate = startDate;

			// Invert duration so that comparisons are more logical
			duration = duration.Negate();

			// Calculate how many weeks we can allocate simultaneously to avoid
			// iterating over days
			long weeks = duration.Ticks / Week.Duration.Ticks;

			if (weeks > 0)
			{
				duration -= TimeSpan.FromTicks(weeks * Week.Duration.Ticks);
				endDate = endDate.AddDays(weeks * 7);
			}

			foreach (Shift shift in DescendingShifts(startDate, DateTime.MinValue))
			{
				if (duration >= shift.Duration)
				{
					// Move the end date to the start of the shift, and subtract the length of the
					// shift from the remaining duration
					endDate = shift.StartTime;
					duration -= shift.Duration;
				}
				else
				{
					// Remaining duration is shorter than the shift
					endDate = shift.EndTime.AddTicks(-duration.Ticks);
					duration -= duration;
				}

				if (duration.Ticks == 0) break;
			}

			return endDate;
		}

		public DateTime DateAdd(DateTime startDate, TimeSpan duration)
		{
			if (duration.Ticks > 0)
			{
				return DateAddPositive(startDate, duration);
			}
			else if (duration.Ticks < 0)
			{
				return DateAddNegative(startDate, duration);
			}
			else
			{
				return startDate;
			}
		}

		#endregion
	}
}
