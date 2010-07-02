using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkingCalendar
{
	/// <summary>
	/// Immutable representation of a working shift, containing a start time and a duration.
	/// </summary>
	public sealed class Shift : IComparable
	{
		#region Properties

		/// <summary>
		/// Time at which the shift starts.
		/// </summary>
		public DateTime StartTime
		{
			get;
			private set;
		}
		
		/// <summary>
		/// Time at which the shift ends.
		/// </summary>
		public DateTime EndTime
		{
			get
			{
				return StartTime.AddTicks(Duration.Ticks);
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

		#endregion

		#region Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="start">Start time.</param>
		/// <param name="duration">Duration of the shift.</param>
		public Shift(DateTime start, TimeSpan duration)
		{
			this.StartTime = start;
			this.Duration = duration;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="hour">Hour at which the shift starts.</param>
		/// <param name="minute">Minute at which the shift starts.</param>
		/// <param name="second">Second at which the shift starts.</param>
		/// <param name="millisecond">Millisecond at which the shift starts.</param>
		/// <param name="duration">Duration of the shift.</param>
		public Shift(double hour, double minute, double second, double millisecond, TimeSpan duration)
		{
			StartTime = DateTime.MinValue.AddHours(hour).AddMinutes(minute).AddSeconds(second).AddMilliseconds(millisecond);
			Duration = duration;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Compare the shift with another.  Comparison is based on start time.  If start times are the same,
		/// comparison is based on duration.
		/// </summary>
		/// <param name="obj">Object to compare with.  Should be another shift.</param>
		/// <returns>Comparison based on start time, then duration.</returns>
		public int CompareTo(object obj)
		{
			if (!obj.GetType().Equals(this.GetType()))
			{
				throw new ArgumentException("Object is not a Shift.");
			}

			Shift shift = (Shift)obj;
			int comparison = StartTime.CompareTo(shift.StartTime);

			if (comparison != 0) return comparison;

			// Start times are the same, so compare based on duration.
			return Duration.CompareTo(shift.Duration);
		}

		#endregion
	}
}