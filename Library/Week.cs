using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkingCalendar
{
	public sealed class Week
	{
		#region Members

		Day[] mDays = new Day[7];

		#endregion

		#region Properties

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

		#region Event Handlers

		void ShiftAdded(Day sender, Shift shift)
		{
			Duration += shift.Duration;
		}

		void ShiftRemoved(Day sender, Shift shift)
		{
			Duration -= shift.Duration;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Get a day by its DayOfWeek value.
		/// </summary>
		/// <param name="dayOfWeek">Day of the week to retrieve.</param>
		/// <returns>The specified day of the week.</returns>
		public Day GetDay(DayOfWeek dayOfWeek)
		{
			return mDays[(int)dayOfWeek];
		}

		#endregion
	}
}
