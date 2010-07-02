﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkingCalendar
{
	/// <summary>
	/// Represents a working day.
	/// </summary>
	public sealed class Day
	{
		#region Delegates

		/// <summary>
		/// Signature for events fired when the shifts are altered.
		/// </summary>
		/// <param name="sender">The day containing the shift.</param>
		/// <param name="shift">The shift that was altered.</param>
		public delegate void ShiftsAlteredEventHandler(Day sender, Shift shift);

		#endregion

		#region Properties

		/// <summary>
		/// The day of the week represented by this day.
		/// </summary>
		public DayOfWeek DayOfWeek
		{
			get;
			set;
		}

		/// <summary>
		/// Collection of shifts.
		/// </summary>
		private ShiftCollection Shifts
		{
			get;
			set;
		}

		/// <summary>
		/// Check if the day is a working day or not.  It is a working day if it contains
		/// any shifts.
		/// </summary>
		public bool IsWorking
		{
			get
			{
				return Shifts.Count > 0;
			}
		}

		/// <summary>
		/// Duration of the shift.
		/// </summary>
		public TimeSpan Duration
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

		#region Events

		/// <summary>
		/// Fired when a new shift is added to the day.
		/// </summary>
		public event ShiftsAlteredEventHandler ShiftAdded;

		/// <summary>
		/// Fired when a shift is removed from the day.
		/// </summary>
		public event ShiftsAlteredEventHandler ShiftRemoved;

		#endregion

		#endregion

		#region Constructors

		public Day(DayOfWeek day)
		{
			DayOfWeek = day;
			Shifts = new ShiftCollection();
			Duration = new TimeSpan();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Add a new shift to the day.  Shifts cannot overlap.
		/// </summary>
		/// <param name="hour">Start hour of the shift.</param>
		/// <param name="minute">Start minute of the shift.</param>
		/// <param name="second">Start second of the shift.</param>
		/// <param name="millisecond">Start millisecond of the shift.</param>
		/// <param name="duration">Duration of the shift.</param>
		public void AddShift(double hour, double minute, double second, double millisecond, TimeSpan duration)
		{
			DateTime startDate = DateTime.MinValue.AddHours(hour).AddMinutes(minute).AddSeconds(second).AddMilliseconds(millisecond);

			// Ensure that this shift does not conflict with an existing shift
			if (IsWorkingTime(startDate)) {
				throw new ArgumentException("New shift conflicts with existing shift.");
			}

			Shift shift = new Shift(hour, minute, second, millisecond, duration);

			// Ensure shifts are inserted in sorted order
			bool inserted = false;

			for (int i = 0; i < Shifts.Count; ++i)
			{
				if (shift.CompareTo(Shifts[i]) < 0)
				{
					Shifts.Insert(i, shift);
					inserted = true;
					break;
				}
			}

			if (!inserted)
			{
				// Append to end
				Shifts.Add(shift);
			}

			Duration += duration;

			// Trigger the shift added event
			if (ShiftAdded != null)
			{
				ShiftAdded(this, shift);
			}
		}

		/// <summary>
		/// Remove the shift with the specified start time, if one exists.
		/// </summary>
		/// <param name="hour">Hour at which the shift starts.</param>
		/// <param name="minute">Minute at which the shift starts.</param>
		/// <param name="second">Second at which the shift starts.</param>
		/// <param name="millisecond">Millisecond at which the shift starts.</param>
		public void RemoveShift(double hour, double minute, double second, double millisecond)
		{
			Shift shift = null;

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
					break;
				}
			}

			if ((ShiftRemoved != null) && (shift != null))
			{
				ShiftRemoved(this, shift);
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
		/// <param name="date">The date to search for.</param>
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
		/// Get the next shift after the supplied date.  If the next appropriate shift contains the supplied date, that shift is returned.
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
