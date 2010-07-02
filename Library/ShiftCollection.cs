using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkingCalendar
{
	public class ShiftCollection : List<Shift>
	{
		public long Duration
		{
			get
			{
				long duration = 0;

				foreach (Shift shift in this)
				{
					duration += shift.Duration;
				}

				return duration;
			}
		}
	}
}
