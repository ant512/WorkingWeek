using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkingCalendar
{
	public class Shift
	{
		#region Properties

		public DateTime StartTime
		{
			get;
			set;
		}
		
		public DateTime EndTime
		{
			get
			{
				return StartTime.AddTicks(Duration);
			}
		}

		public long Duration
		{
			get;
			set;
		}

		#endregion

		#region Constructors

		public Shift(DateTime start, long duration)
		{
			this.StartTime = start;
			this.Duration = duration;
		}

		public Shift(double hour, double minute, double second, double millisecond, long duration)
		{
			StartTime = DateTime.MinValue.AddHours(hour).AddMinutes(minute).AddSeconds(second).AddMilliseconds(millisecond);
			Duration = duration;
		}

		#endregion
	}
}