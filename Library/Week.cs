using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkingCalendar
{
	public class Week
	{
		#region Members

		Day[] mDays = new Day[7];

		#endregion

		#region Properties

		public long Duration
		{
			get
			{
				long duration = 0;

				foreach (Day day in mDays)
				{
					duration += day.Duration;
				}

				return duration;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		public Week()
		{
			for (int i = 0; i < 7; ++i)
			{
				mDays[i] = new Day((DayOfWeek)i);
			}
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
