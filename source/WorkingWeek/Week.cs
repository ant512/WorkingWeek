using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkingWeek
{
	/// <summary>
	/// Represents a working week.
	/// </summary>
	public sealed class Week
	{
		#region Members

		/// <summary>
		/// Array of days in the week.
		/// </summary>
		private Day[] mDays = new Day[7];

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the Week class.
		/// </summary>
		public Week()
		{
			Duration = new TimeSpan();

			for (int i = 0; i < 7; ++i)
			{
				mDays[i] = new Day((DayOfWeek)i);
				mDays[i].ShiftAdded += ShiftAdded;
				mDays[i].ShiftRemoved += ShiftRemoved;
			}
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the total duration of the work week.
		/// </summary>
		public TimeSpan Duration { get; private set; }

		/// <summary>
		/// Gets a value indicating whether the week contains any shifts.
		/// </summary>
		public bool ContainsShifts
		{
			get { return mDays.Any(d => d.IsWorking); }
		}

		#endregion

		#region Event Handlers

		/// <summary>
		/// Called when a new shift is added to one of the week's days.  Increases the
		/// duraton of the week by the duration of the shift.
		/// </summary>
		/// <param name="shift">The added shift.</param>
		public void ShiftAdded(Shift shift)
		{
			Duration += shift.Duration;
		}

		/// <summary>
		/// Called when a shift is removed from one of the week's days.  Reduces the
		/// duration of the week by the duration of the shift.
		/// </summary>
		/// <param name="shift">The removed shift.</param>
		public void ShiftRemoved(Shift shift)
		{
			Duration -= shift.Duration;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Add a new shift to the specified day.  Shifts cannot overlap.
		/// </summary>
		/// <param name="dayOfWeek">Day on which to add the shift.</param>
		/// <param name="hour">Start hour of the shift.</param>
		/// <param name="minute">Start minute of the shift.</param>
		/// <param name="second">Start second of the shift.</param>
		/// <param name="millisecond">Start millisecond of the shift.</param>
		/// <param name="duration">Duration of the shift.</param>
		public void AddShift(DayOfWeek dayOfWeek, double hour, double minute, double second, double millisecond, TimeSpan duration)
		{
			GetDay(dayOfWeek).AddShift(hour, minute, second, millisecond, duration);
		}

		/// <summary>
		/// Remove the shift with the specified start time, if one exists.
		/// </summary>
		/// <param name="dayOfWeek">Day from which to remove the shift.</param>
		/// <param name="hour">Hour at which the shift starts.</param>
		/// <param name="minute">Minute at which the shift starts.</param>
		/// <param name="second">Second at which the shift starts.</param>
		/// <param name="millisecond">Millisecond at which the shift starts.</param>
		public void RemoveShift(DayOfWeek dayOfWeek, double hour, double minute, double second, double millisecond)
		{
			GetDay(dayOfWeek).RemoveShift(hour, minute, second, millisecond);
		}

		/// <summary>
		/// Check if the given date/time falls within a shift.
		/// </summary>
		/// <param name="date">The date to check.</param>
		/// <returns>True if the date falls within a shift.</returns>
		public bool IsWorking(DateTime date)
		{
			Day workingDay = GetDay(date.DayOfWeek);

			if (workingDay == null) return false;

			if (!workingDay.ContainsShifts) return false;

			return workingDay.IsWorkingTime(date);
		}

		/// <summary>
		/// Check if the given day is a working day.
		/// </summary>
		/// <param name="dayOfWeek">The day of the week to check.</param>
		/// <returns>True if the day is a working day.</returns>
		public bool IsWorking(DayOfWeek dayOfWeek)
		{
			return GetDay(dayOfWeek).IsWorking;
		}

		/// <summary>
		/// Custom iterator that produces each shift between the two dates.  Enter Date.MaxValue as
		/// the end date to produce an endless list of shifts.
		/// </summary>
		/// <param name="startDate">The date to start from.</param>
		/// <param name="endDate">The date to end at.</param>
		/// <returns>A list of shifts, in ascending order, from startDate to endDate.</returns>
		public IEnumerable<Shift> AscendingShifts(DateTime startDate, DateTime endDate)
		{
			if (ContainsShifts)
			{
				// Start with the day appropriate to the supplied ate
				Day day = GetDay(startDate.DayOfWeek);
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
							day = GetDay(currentDate.DayOfWeek);
						}
					}

					if (shift != null)
					{
						// Ensure that the shift contains the correct date
						Shift adjustedShift = new Shift(new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, shift.StartTime.Hour, shift.StartTime.Minute, shift.StartTime.Second, shift.StartTime.Millisecond), shift.Duration);
						currentDate = adjustedShift.EndTime;

						// Return the adjusted shift if it is within the valid range
						if (adjustedShift.StartTime < endDate)
						{
							yield return adjustedShift;
						}
					}
				}
			}
		}

		/// <summary>
		/// Custom iterator that produces each shift between the two dates.  Enter Date.MinValue as
		/// the end date to produce an endless list of shifts.
		/// </summary>
		/// <param name="startDate">The date to start from.</param>
		/// <param name="endDate">The date to end at.</param>
		/// <returns>A list of shifts, in descending order, from startDate to endDate.</returns>
		public IEnumerable<Shift> DescendingShifts(DateTime startDate, DateTime endDate)
		{
			if (ContainsShifts)
			{
				// Start with the day appropriate to the supplied ate
				Day day = GetDay(startDate.DayOfWeek);
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
							day = GetDay(currentDate.DayOfWeek);
						}
					}

					if (shift != null)
					{
						// Ensure that the shift contains the correct date
						Shift adjustedShift = new Shift(new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, shift.StartTime.Hour, shift.StartTime.Minute, shift.StartTime.Second, shift.StartTime.Millisecond), shift.Duration);
						currentDate = adjustedShift.StartTime.AddMilliseconds(-1);

						// Return the adjusted shift if it is within the valid range
						if (adjustedShift.StartTime > endDate)
						{
							yield return adjustedShift;
						}
					}
				}
			}
		}

		/// <summary>
		/// Returns the date produced by adding the supplied duration to the supplied date, but only allowing
		/// time to pass during working shifts.
		/// </summary>
		/// <param name="startDate">Start date.</param>
		/// <param name="duration">Duration to add to the date.</param>
		/// <returns>Date produced as a result of adding duration to start date, via way of the shifts in the calendar.</returns>
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

			return startDate;
		}

		/// <summary>
		/// Find the difference, in working time, between the two dates.
		/// </summary>
		/// <param name="startDate">Start date.</param>
		/// <param name="endDate">End date.</param>
		/// <returns>The difference, in working time, between the two dates.</returns>
		public TimeSpan DateDiff(DateTime startDate, DateTime endDate)
		{
			bool invertedDates = false;

			// Invert dates if necessary so that startDate always precedes endDate
			if (startDate > endDate)
			{
				DateTime swap = startDate;
				startDate = endDate;
				endDate = swap;

				invertedDates = true;
			}

			TimeSpan timeDiff = endDate.Subtract(startDate);
			TimeSpan workDiff = new TimeSpan();

			// Calculate how many weeks difference there are between the two dates,
			// then calculate how much work time that represents, and add it to the workDiff
			TimeSpan singleWeek = new TimeSpan(1, 0, 0, 0);
			long weeks = timeDiff.Ticks / (singleWeek.Ticks * 7);

			if (weeks > 0)
			{
				workDiff += TimeSpan.FromTicks(weeks * Duration.Ticks);
				startDate = startDate.AddDays(weeks * 7);
			}

			// Allocate remaining fraction of a week
			foreach (Shift shift in AscendingShifts(startDate, endDate))
			{
				workDiff += shift.Duration;
			}

			// Return inverted timespan if we swapped the dates
			if (!invertedDates)
			{
				return workDiff;
			}
			else
			{
				return workDiff.Negate();
			}
		}

		/// <summary>
		/// Get a day by its DayOfWeek value.
		/// </summary>
		/// <param name="dayOfWeek">Day of the week to retrieve.</param>
		/// <returns>The specified day of the week.</returns>
		private Day GetDay(DayOfWeek dayOfWeek)
		{
			return mDays[(int)dayOfWeek];
		}

		/// <summary>
		/// Adds a positive duration to the supplied start date.
		/// </summary>
		/// <param name="startDate">The date to which to add the duration.</param>
		/// <param name="duration">The duration to add.</param>
		/// <returns>The resulting date.</returns>
		private DateTime DateAddPositive(DateTime startDate, TimeSpan duration)
		{
			System.Diagnostics.Debug.Assert(duration.Ticks > -1, "Duration must not be negative.");

			DateTime endDate = startDate;

			// Calculate how many weeks we can allocate simultaneously to avoid
			// iterating over days
			long weeks = duration.Ticks / Duration.Ticks;

			if (weeks > 0)
			{
				duration -= TimeSpan.FromTicks(weeks * Duration.Ticks);
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

		/// <summary>
		/// Adds a negative duration to the supplied start date.
		/// </summary>
		/// <param name="startDate">The date to which to add the duration.</param>
		/// <param name="duration">The duration to add.</param>
		/// <returns>The resulting date.</returns>
		private DateTime DateAddNegative(DateTime startDate, TimeSpan duration)
		{
			System.Diagnostics.Debug.Assert(duration.Ticks < 1, "Duration must not be positive.");

			DateTime endDate = startDate;

			// Invert duration so that comparisons are more logical
			duration = duration.Negate();

			// Calculate how many weeks we can allocate simultaneously to avoid
			// iterating over days
			long weeks = duration.Ticks / Duration.Ticks;

			if (weeks > 0)
			{
				duration -= TimeSpan.FromTicks(weeks * Duration.Ticks);
				endDate = endDate.AddDays(-weeks * 7);
			}

			foreach (Shift shift in DescendingShifts(endDate, DateTime.MinValue))
			{
				// Stop if we've allocated the entire duration
				if (duration.Ticks == 0) break;

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
			}

			return endDate;
		}

		#endregion
	}
}
