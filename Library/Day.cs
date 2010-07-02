using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkingCalendar
{
	public class Day
	{
		#region Properties

		public DayOfWeek DayOfWeek
		{
			get;
			set;
		}

		private ShiftCollection Shifts
		{
			get;
			set;
		}

		public bool IsWorking
		{
			get
			{
				return Shifts.Count > 0;
			}
		}

		public long Duration
		{
			get;
			private set;
		}

		/// <summary>
		/// Check if this day contains any shifts.
		/// </summary>
		/// <returns></returns>
		public bool ContainsShifts
		{
			get { return Shifts.Count > 0; }
		}

		#endregion

		#region Constructors

		public Day(DayOfWeek day)
		{
			DayOfWeek = day;
			Shifts = new ShiftCollection();
			Duration = 0;
		}

		#endregion

		#region Methods

		public void AddShift(double hour, double minute, double second, double millisecond, long duration)
		{
			DateTime startDate = DateTime.MinValue.AddHours(hour).AddMinutes(minute).AddSeconds(second).AddMilliseconds(millisecond);

			// Ensure that this shift does not conflict with an existing shift
			foreach (Shift shift in Shifts)
			{
				if ((shift.StartTime <= startDate) && (shift.EndTime >= startDate))
				{
					throw new ArgumentException("New shift conflicts with existing shift.");
				}
			}

			Shifts.Add(new Shift(hour, minute, second, millisecond, duration));
			Duration += duration;
		}

		public void RemoveShift(double hour, double minute, double second, double millisecond)
		{
			Shift shift;

			// Locate the shift in the array
			for (int i = 0; i < Shifts.Count; ++i)
			{
				shift = Shifts[i];

				if ((shift.StartTime.Hour == hour) &&
					(shift.StartTime.Minute == minute) &&
					(shift.StartTime.Second == second) &&
					(shift.StartTime.Millisecond == millisecond))
				{
					// Ensure that we remove the shift's duration from the overall day duration before we
					// remove it
					Duration -= shift.Duration;
					Shifts.Remove(shift);
					return;
				}
			}
		}


		/// <summary>
		/// Check if the supplied date is contained within a working shift.
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public bool IsWorkingTime(DateTime date)
		{
			return (FindShift(date) != null);
		}

		/// <summary>
		/// Finds the shift that contains the supplied date.  Only the hour, minute, second and milliseconds of the date are considered
		/// when matching the date, so the day, year, etc can be any value.  The assumption is that the working shift pattern will be the
		/// same for any monday, or any tuesday, and so on.
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public Shift FindShift(DateTime date)
		{
			DateTime searchTime = DateTime.MinValue.AddHours(date.Hour).AddMinutes(date.Minute).AddSeconds(date.Second).AddMilliseconds(date.Millisecond);

			foreach (Shift shift in Shifts)
			{
				if ((searchTime >= shift.StartTime) && (searchTime < shift.EndTime)) return shift;
			}

			return null;
		}

		/// <summary>
		/// Get the next shift after the supplied date.
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public Shift GetNextShift(DateTime date)
		{
			DateTime searchTime = DateTime.MinValue.AddHours(date.Hour).AddMinutes(date.Minute).AddSeconds(date.Second).AddMilliseconds(date.Millisecond);

			foreach (Shift shift in Shifts)
			{
				if (searchTime <= shift.StartTime) return shift;
			}

			return null;
		}

		/// <summary>
		/// Get the shift prior to the supplied date.  If the next appropriate shift contains the supplied date, that shift is returned.
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public Shift GetPreviousShift(DateTime date)
		{
			DateTime searchTime = DateTime.MinValue.AddHours(date.Hour).AddMinutes(date.Minute).AddSeconds(date.Second).AddMilliseconds(date.Millisecond);
			Shift shift;

			for (int i = Shifts.Count - 1; i >= 0; --i)
			{
				shift = Shifts[i];

				if (searchTime >= shift.StartTime) return shift;
			}

			return null;
		}

		#endregion
	}
}
