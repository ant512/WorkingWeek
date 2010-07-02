using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkingCalendar
{
	public class DayCollection
	{
		#region Members

		Day[] mWorkingDays = new Day[7];

		#endregion

		#region Properties

		public long Duration
		{
			get
			{
				long duration = 0;

				foreach (Day day in mWorkingDays)
				{
					duration += day.Duration;
				}

				return duration;
			}
		}

		#endregion

		#region Constructors

		public DayCollection()
		{
			for (int i = 0; i < 7; ++i)
			{
				mWorkingDays[i] = new Day((DayOfWeek)i);
			}
		}

		#endregion

		#region Methods

		public Day GetDay(DayOfWeek dayOfWeek)
		{
			return mWorkingDays[(int)dayOfWeek];
		}

		#endregion
	}
}
