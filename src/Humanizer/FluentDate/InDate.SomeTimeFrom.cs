 
#if NET6_0_OR_GREATER

using System;

namespace Humanizer
{
    /// <summary>
    /// </summary>
    public partial class InDate
    {
   
        /// <summary>
        /// </summary>
		public static class One
		{
			/// <summary>
			/// 1 days from now
			/// </summary>
			public static DateOnly Day
			{
				get { return DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)); }
			}

			/// <summary>
			/// 1 days from the provided date
			/// </summary>
			public static DateOnly DayFrom(DateOnly date)
			{
				return date.AddDays(1);
			}

            /// <summary>
			/// 1 days from the provided date
			/// </summary>
			public static DateOnly DayFrom(DateTime date)
			{
				return DateOnly.FromDateTime(date.AddDays(1));
			}

			/// <summary>
			/// 1 weeks from now
			/// </summary>
			public static DateOnly Week
			{
				get { return DateOnly.FromDateTime(DateTime.UtcNow.AddDays(7)); }
			}

			/// <summary>
			/// 1 weeks from the provided date
			/// </summary>
			public static DateOnly WeekFrom(DateOnly date)
			{
				return date.AddDays(7);
			}

			/// <summary>
			/// 1 weeks from the provided date
			/// </summary>
			public static DateOnly WeekFrom(DateTime date)
			{
				return DateOnly.FromDateTime(date.AddDays(7));
			}

			/// <summary>
			/// 1 months from now
			/// </summary>
			public static DateOnly Month
			{
				get { return DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(1)); }
			}

			/// <summary>
			/// 1 months from the provided date
			/// </summary>
			public static DateOnly MonthFrom(DateOnly date)
			{
				return date.AddMonths(1);
			}

            /// <summary>
			/// 1 months from the provided date
			/// </summary>
			public static DateOnly MonthFrom(DateTime date)
			{
				return DateOnly.FromDateTime(date.AddMonths(1));
			}

			/// <summary>
			/// 1 years from now
			/// </summary>
			public static DateOnly Year
			{
				get { return DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1)); }
			}

			/// <summary>
			/// 1 years from the provided date
			/// </summary>
			public static DateOnly YearFrom(DateOnly date)
			{
				return date.AddYears(1);
			}

			/// <summary>
			/// 1 years from the provided date
			/// </summary>
			public static DateOnly YearFrom(DateTime date)
			{
				return DateOnly.FromDateTime(date.AddYears(1));
			}
		}
     
        /// <summary>
        /// </summary>
		public static class Two
		{
			/// <summary>
			/// 2 days from now
			/// </summary>
			public static DateOnly Days
			{
				get { return DateOnly.FromDateTime(DateTime.UtcNow.AddDays(2)); }
			}

			/// <summary>
			/// 2 days from the provided date
			/// </summary>
			public static DateOnly DaysFrom(DateOnly date)
			{
				return date.AddDays(2);
			}

            /// <summary>
			/// 2 days from the provided date
			/// </summary>
			public static DateOnly DaysFrom(DateTime date)
			{
				return DateOnly.FromDateTime(date.AddDays(2));
			}

			/// <summary>
			/// 2 weeks from now
			/// </summary>
			public static DateOnly Weeks
			{
				get { return DateOnly.FromDateTime(DateTime.UtcNow.AddDays(14)); }
			}

			/// <summary>
			/// 2 weeks from the provided date
			/// </summary>
			public static DateOnly WeeksFrom(DateOnly date)
			{
				return date.AddDays(14);
			}

			/// <summary>
			/// 2 weeks from the provided date
			/// </summary>
			public static DateOnly WeeksFrom(DateTime date)
			{
				return DateOnly.FromDateTime(date.AddDays(14));
			}

			/// <summary>
			/// 2 months from now
			/// </summary>
			public static DateOnly Months
			{
				get { return DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(2)); }
			}

			/// <summary>
			/// 2 months from the provided date
			/// </summary>
			public static DateOnly MonthsFrom(DateOnly date)
			{
				return date.AddMonths(2);
			}

            /// <summary>
			/// 2 months from the provided date
			/// </summary>
			public static DateOnly MonthsFrom(DateTime date)
			{
				return DateOnly.FromDateTime(date.AddMonths(2));
			}

			/// <summary>
			/// 2 years from now
			/// </summary>
			public static DateOnly Years
			{
				get { return DateOnly.FromDateTime(DateTime.UtcNow.AddYears(2)); }
			}

			/// <summary>
			/// 2 years from the provided date
			/// </summary>
			public static DateOnly YearsFrom(DateOnly date)
			{
				return date.AddYears(2);
			}

			/// <summary>
			/// 2 years from the provided date
			/// </summary>
			public static DateOnly YearsFrom(DateTime date)
			{
				return DateOnly.FromDateTime(date.AddYears(2));
			}
		}
     
        /// <summary>
        /// </summary>
		public static class Three
		{
			/// <summary>
			/// 3 days from now
			/// </summary>
			public static DateOnly Days
			{
				get { return DateOnly.FromDateTime(DateTime.UtcNow.AddDays(3)); }
			}

			/// <summary>
			/// 3 days from the provided date
			/// </summary>
			public static DateOnly DaysFrom(DateOnly date)
			{
				return date.AddDays(3);
			}

            /// <summary>
			/// 3 days from the provided date
			/// </summary>
			public static DateOnly DaysFrom(DateTime date)
			{
				return DateOnly.FromDateTime(date.AddDays(3));
			}

			/// <summary>
			/// 3 weeks from now
			/// </summary>
			public static DateOnly Weeks
			{
				get { return DateOnly.FromDateTime(DateTime.UtcNow.AddDays(21)); }
			}

			/// <summary>
			/// 3 weeks from the provided date
			/// </summary>
			public static DateOnly WeeksFrom(DateOnly date)
			{
				return date.AddDays(21);
			}

			/// <summary>
			/// 3 weeks from the provided date
			/// </summary>
			public static DateOnly WeeksFrom(DateTime date)
			{
				return DateOnly.FromDateTime(date.AddDays(21));
			}

			/// <summary>
			/// 3 months from now
			/// </summary>
			public static DateOnly Months
			{
				get { return DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(3)); }
			}

			/// <summary>
			/// 3 months from the provided date
			/// </summary>
			public static DateOnly MonthsFrom(DateOnly date)
			{
				return date.AddMonths(3);
			}

            /// <summary>
			/// 3 months from the provided date
			/// </summary>
			public static DateOnly MonthsFrom(DateTime date)
			{
				return DateOnly.FromDateTime(date.AddMonths(3));
			}

			/// <summary>
			/// 3 years from now
			/// </summary>
			public static DateOnly Years
			{
				get { return DateOnly.FromDateTime(DateTime.UtcNow.AddYears(3)); }
			}

			/// <summary>
			/// 3 years from the provided date
			/// </summary>
			public static DateOnly YearsFrom(DateOnly date)
			{
				return date.AddYears(3);
			}

			/// <summary>
			/// 3 years from the provided date
			/// </summary>
			public static DateOnly YearsFrom(DateTime date)
			{
				return DateOnly.FromDateTime(date.AddYears(3));
			}
		}
     
        /// <summary>
        /// </summary>
		public static class Four
		{
			/// <summary>
			/// 4 days from now
			/// </summary>
			public static DateOnly Days
			{
				get { return DateOnly.FromDateTime(DateTime.UtcNow.AddDays(4)); }
			}

			/// <summary>
			/// 4 days from the provided date
			/// </summary>
			public static DateOnly DaysFrom(DateOnly date)
			{
				return date.AddDays(4);
			}

            /// <summary>
			/// 4 days from the provided date
			/// </summary>
			public static DateOnly DaysFrom(DateTime date)
			{
				return DateOnly.FromDateTime(date.AddDays(4));
			}

			/// <summary>
			/// 4 weeks from now
			/// </summary>
			public static DateOnly Weeks
			{
				get { return DateOnly.FromDateTime(DateTime.UtcNow.AddDays(28)); }
			}

			/// <summary>
			/// 4 weeks from the provided date
			/// </summary>
			public static DateOnly WeeksFrom(DateOnly date)
			{
				return date.AddDays(28);
			}

			/// <summary>
			/// 4 weeks from the provided date
			/// </summary>
			public static DateOnly WeeksFrom(DateTime date)
			{
				return DateOnly.FromDateTime(date.AddDays(28));
			}

			/// <summary>
			/// 4 months from now
			/// </summary>
			public static DateOnly Months
			{
				get { return DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(4)); }
			}

			/// <summary>
			/// 4 months from the provided date
			/// </summary>
			public static DateOnly MonthsFrom(DateOnly date)
			{
				return date.AddMonths(4);
			}

            /// <summary>
			/// 4 months from the provided date
			/// </summary>
			public static DateOnly MonthsFrom(DateTime date)
			{
				return DateOnly.FromDateTime(date.AddMonths(4));
			}

			/// <summary>
			/// 4 years from now
			/// </summary>
			public static DateOnly Years
			{
				get { return DateOnly.FromDateTime(DateTime.UtcNow.AddYears(4)); }
			}

			/// <summary>
			/// 4 years from the provided date
			/// </summary>
			public static DateOnly YearsFrom(DateOnly date)
			{
				return date.AddYears(4);
			}

			/// <summary>
			/// 4 years from the provided date
			/// </summary>
			public static DateOnly YearsFrom(DateTime date)
			{
				return DateOnly.FromDateTime(date.AddYears(4));
			}
		}
     
        /// <summary>
        /// </summary>
		public static class Five
		{
			/// <summary>
			/// 5 days from now
			/// </summary>
			public static DateOnly Days
			{
				get { return DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5)); }
			}

			/// <summary>
			/// 5 days from the provided date
			/// </summary>
			public static DateOnly DaysFrom(DateOnly date)
			{
				return date.AddDays(5);
			}

            /// <summary>
			/// 5 days from the provided date
			/// </summary>
			public static DateOnly DaysFrom(DateTime date)
			{
				return DateOnly.FromDateTime(date.AddDays(5));
			}

			/// <summary>
			/// 5 weeks from now
			/// </summary>
			public static DateOnly Weeks
			{
				get { return DateOnly.FromDateTime(DateTime.UtcNow.AddDays(35)); }
			}

			/// <summary>
			/// 5 weeks from the provided date
			/// </summary>
			public static DateOnly WeeksFrom(DateOnly date)
			{
				return date.AddDays(35);
			}

			/// <summary>
			/// 5 weeks from the provided date
			/// </summary>
			public static DateOnly WeeksFrom(DateTime date)
			{
				return DateOnly.FromDateTime(date.AddDays(35));
			}

			/// <summary>
			/// 5 months from now
			/// </summary>
			public static DateOnly Months
			{
				get { return DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(5)); }
			}

			/// <summary>
			/// 5 months from the provided date
			/// </summary>
			public static DateOnly MonthsFrom(DateOnly date)
			{
				return date.AddMonths(5);
			}

            /// <summary>
			/// 5 months from the provided date
			/// </summary>
			public static DateOnly MonthsFrom(DateTime date)
			{
				return DateOnly.FromDateTime(date.AddMonths(5));
			}

			/// <summary>
			/// 5 years from now
			/// </summary>
			public static DateOnly Years
			{
				get { return DateOnly.FromDateTime(DateTime.UtcNow.AddYears(5)); }
			}

			/// <summary>
			/// 5 years from the provided date
			/// </summary>
			public static DateOnly YearsFrom(DateOnly date)
			{
				return date.AddYears(5);
			}

			/// <summary>
			/// 5 years from the provided date
			/// </summary>
			public static DateOnly YearsFrom(DateTime date)
			{
				return DateOnly.FromDateTime(date.AddYears(5));
			}
		}
     
        /// <summary>
        /// </summary>
		public static class Six
		{
			/// <summary>
			/// 6 days from now
			/// </summary>
			public static DateOnly Days
			{
				get { return DateOnly.FromDateTime(DateTime.UtcNow.AddDays(6)); }
			}

			/// <summary>
			/// 6 days from the provided date
			/// </summary>
			public static DateOnly DaysFrom(DateOnly date)
			{
				return date.AddDays(6);
			}

            /// <summary>
			/// 6 days from the provided date
			/// </summary>
			public static DateOnly DaysFrom(DateTime date)
			{
				return DateOnly.FromDateTime(date.AddDays(6));
			}

			/// <summary>
			/// 6 weeks from now
			/// </summary>
			public static DateOnly Weeks
			{
				get { return DateOnly.FromDateTime(DateTime.UtcNow.AddDays(42)); }
			}

			/// <summary>
			/// 6 weeks from the provided date
			/// </summary>
			public static DateOnly WeeksFrom(DateOnly date)
			{
				return date.AddDays(42);
			}

			/// <summary>
			/// 6 weeks from the provided date
			/// </summary>
			public static DateOnly WeeksFrom(DateTime date)
			{
				return DateOnly.FromDateTime(date.AddDays(42));
			}

			/// <summary>
			/// 6 months from now
			/// </summary>
			public static DateOnly Months
			{
				get { return DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(6)); }
			}

			/// <summary>
			/// 6 months from the provided date
			/// </summary>
			public static DateOnly MonthsFrom(DateOnly date)
			{
				return date.AddMonths(6);
			}

            /// <summary>
			/// 6 months from the provided date
			/// </summary>
			public static DateOnly MonthsFrom(DateTime date)
			{
				return DateOnly.FromDateTime(date.AddMonths(6));
			}

			/// <summary>
			/// 6 years from now
			/// </summary>
			public static DateOnly Years
			{
				get { return DateOnly.FromDateTime(DateTime.UtcNow.AddYears(6)); }
			}

			/// <summary>
			/// 6 years from the provided date
			/// </summary>
			public static DateOnly YearsFrom(DateOnly date)
			{
				return date.AddYears(6);
			}

			/// <summary>
			/// 6 years from the provided date
			/// </summary>
			public static DateOnly YearsFrom(DateTime date)
			{
				return DateOnly.FromDateTime(date.AddYears(6));
			}
		}
     
        /// <summary>
        /// </summary>
		public static class Seven
		{
			/// <summary>
			/// 7 days from now
			/// </summary>
			public static DateOnly Days
			{
				get { return DateOnly.FromDateTime(DateTime.UtcNow.AddDays(7)); }
			}

			/// <summary>
			/// 7 days from the provided date
			/// </summary>
			public static DateOnly DaysFrom(DateOnly date)
			{
				return date.AddDays(7);
			}

            /// <summary>
			/// 7 days from the provided date
			/// </summary>
			public static DateOnly DaysFrom(DateTime date)
			{
				return DateOnly.FromDateTime(date.AddDays(7));
			}

			/// <summary>
			/// 7 weeks from now
			/// </summary>
			public static DateOnly Weeks
			{
				get { return DateOnly.FromDateTime(DateTime.UtcNow.AddDays(49)); }
			}

			/// <summary>
			/// 7 weeks from the provided date
			/// </summary>
			public static DateOnly WeeksFrom(DateOnly date)
			{
				return date.AddDays(49);
			}

			/// <summary>
			/// 7 weeks from the provided date
			/// </summary>
			public static DateOnly WeeksFrom(DateTime date)
			{
				return DateOnly.FromDateTime(date.AddDays(49));
			}

			/// <summary>
			/// 7 months from now
			/// </summary>
			public static DateOnly Months
			{
				get { return DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(7)); }
			}

			/// <summary>
			/// 7 months from the provided date
			/// </summary>
			public static DateOnly MonthsFrom(DateOnly date)
			{
				return date.AddMonths(7);
			}

            /// <summary>
			/// 7 months from the provided date
			/// </summary>
			public static DateOnly MonthsFrom(DateTime date)
			{
				return DateOnly.FromDateTime(date.AddMonths(7));
			}

			/// <summary>
			/// 7 years from now
			/// </summary>
			public static DateOnly Years
			{
				get { return DateOnly.FromDateTime(DateTime.UtcNow.AddYears(7)); }
			}

			/// <summary>
			/// 7 years from the provided date
			/// </summary>
			public static DateOnly YearsFrom(DateOnly date)
			{
				return date.AddYears(7);
			}

			/// <summary>
			/// 7 years from the provided date
			/// </summary>
			public static DateOnly YearsFrom(DateTime date)
			{
				return DateOnly.FromDateTime(date.AddYears(7));
			}
		}
     
        /// <summary>
        /// </summary>
		public static class Eight
		{
			/// <summary>
			/// 8 days from now
			/// </summary>
			public static DateOnly Days
			{
				get { return DateOnly.FromDateTime(DateTime.UtcNow.AddDays(8)); }
			}

			/// <summary>
			/// 8 days from the provided date
			/// </summary>
			public static DateOnly DaysFrom(DateOnly date)
			{
				return date.AddDays(8);
			}

            /// <summary>
			/// 8 days from the provided date
			/// </summary>
			public static DateOnly DaysFrom(DateTime date)
			{
				return DateOnly.FromDateTime(date.AddDays(8));
			}

			/// <summary>
			/// 8 weeks from now
			/// </summary>
			public static DateOnly Weeks
			{
				get { return DateOnly.FromDateTime(DateTime.UtcNow.AddDays(56)); }
			}

			/// <summary>
			/// 8 weeks from the provided date
			/// </summary>
			public static DateOnly WeeksFrom(DateOnly date)
			{
				return date.AddDays(56);
			}

			/// <summary>
			/// 8 weeks from the provided date
			/// </summary>
			public static DateOnly WeeksFrom(DateTime date)
			{
				return DateOnly.FromDateTime(date.AddDays(56));
			}

			/// <summary>
			/// 8 months from now
			/// </summary>
			public static DateOnly Months
			{
				get { return DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(8)); }
			}

			/// <summary>
			/// 8 months from the provided date
			/// </summary>
			public static DateOnly MonthsFrom(DateOnly date)
			{
				return date.AddMonths(8);
			}

            /// <summary>
			/// 8 months from the provided date
			/// </summary>
			public static DateOnly MonthsFrom(DateTime date)
			{
				return DateOnly.FromDateTime(date.AddMonths(8));
			}

			/// <summary>
			/// 8 years from now
			/// </summary>
			public static DateOnly Years
			{
				get { return DateOnly.FromDateTime(DateTime.UtcNow.AddYears(8)); }
			}

			/// <summary>
			/// 8 years from the provided date
			/// </summary>
			public static DateOnly YearsFrom(DateOnly date)
			{
				return date.AddYears(8);
			}

			/// <summary>
			/// 8 years from the provided date
			/// </summary>
			public static DateOnly YearsFrom(DateTime date)
			{
				return DateOnly.FromDateTime(date.AddYears(8));
			}
		}
     
        /// <summary>
        /// </summary>
		public static class Nine
		{
			/// <summary>
			/// 9 days from now
			/// </summary>
			public static DateOnly Days
			{
				get { return DateOnly.FromDateTime(DateTime.UtcNow.AddDays(9)); }
			}

			/// <summary>
			/// 9 days from the provided date
			/// </summary>
			public static DateOnly DaysFrom(DateOnly date)
			{
				return date.AddDays(9);
			}

            /// <summary>
			/// 9 days from the provided date
			/// </summary>
			public static DateOnly DaysFrom(DateTime date)
			{
				return DateOnly.FromDateTime(date.AddDays(9));
			}

			/// <summary>
			/// 9 weeks from now
			/// </summary>
			public static DateOnly Weeks
			{
				get { return DateOnly.FromDateTime(DateTime.UtcNow.AddDays(63)); }
			}

			/// <summary>
			/// 9 weeks from the provided date
			/// </summary>
			public static DateOnly WeeksFrom(DateOnly date)
			{
				return date.AddDays(63);
			}

			/// <summary>
			/// 9 weeks from the provided date
			/// </summary>
			public static DateOnly WeeksFrom(DateTime date)
			{
				return DateOnly.FromDateTime(date.AddDays(63));
			}

			/// <summary>
			/// 9 months from now
			/// </summary>
			public static DateOnly Months
			{
				get { return DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(9)); }
			}

			/// <summary>
			/// 9 months from the provided date
			/// </summary>
			public static DateOnly MonthsFrom(DateOnly date)
			{
				return date.AddMonths(9);
			}

            /// <summary>
			/// 9 months from the provided date
			/// </summary>
			public static DateOnly MonthsFrom(DateTime date)
			{
				return DateOnly.FromDateTime(date.AddMonths(9));
			}

			/// <summary>
			/// 9 years from now
			/// </summary>
			public static DateOnly Years
			{
				get { return DateOnly.FromDateTime(DateTime.UtcNow.AddYears(9)); }
			}

			/// <summary>
			/// 9 years from the provided date
			/// </summary>
			public static DateOnly YearsFrom(DateOnly date)
			{
				return date.AddYears(9);
			}

			/// <summary>
			/// 9 years from the provided date
			/// </summary>
			public static DateOnly YearsFrom(DateTime date)
			{
				return DateOnly.FromDateTime(date.AddYears(9));
			}
		}
     
        /// <summary>
        /// </summary>
		public static class Ten
		{
			/// <summary>
			/// 10 days from now
			/// </summary>
			public static DateOnly Days
			{
				get { return DateOnly.FromDateTime(DateTime.UtcNow.AddDays(10)); }
			}

			/// <summary>
			/// 10 days from the provided date
			/// </summary>
			public static DateOnly DaysFrom(DateOnly date)
			{
				return date.AddDays(10);
			}

            /// <summary>
			/// 10 days from the provided date
			/// </summary>
			public static DateOnly DaysFrom(DateTime date)
			{
				return DateOnly.FromDateTime(date.AddDays(10));
			}

			/// <summary>
			/// 10 weeks from now
			/// </summary>
			public static DateOnly Weeks
			{
				get { return DateOnly.FromDateTime(DateTime.UtcNow.AddDays(70)); }
			}

			/// <summary>
			/// 10 weeks from the provided date
			/// </summary>
			public static DateOnly WeeksFrom(DateOnly date)
			{
				return date.AddDays(70);
			}

			/// <summary>
			/// 10 weeks from the provided date
			/// </summary>
			public static DateOnly WeeksFrom(DateTime date)
			{
				return DateOnly.FromDateTime(date.AddDays(70));
			}

			/// <summary>
			/// 10 months from now
			/// </summary>
			public static DateOnly Months
			{
				get { return DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(10)); }
			}

			/// <summary>
			/// 10 months from the provided date
			/// </summary>
			public static DateOnly MonthsFrom(DateOnly date)
			{
				return date.AddMonths(10);
			}

            /// <summary>
			/// 10 months from the provided date
			/// </summary>
			public static DateOnly MonthsFrom(DateTime date)
			{
				return DateOnly.FromDateTime(date.AddMonths(10));
			}

			/// <summary>
			/// 10 years from now
			/// </summary>
			public static DateOnly Years
			{
				get { return DateOnly.FromDateTime(DateTime.UtcNow.AddYears(10)); }
			}

			/// <summary>
			/// 10 years from the provided date
			/// </summary>
			public static DateOnly YearsFrom(DateOnly date)
			{
				return date.AddYears(10);
			}

			/// <summary>
			/// 10 years from the provided date
			/// </summary>
			public static DateOnly YearsFrom(DateTime date)
			{
				return DateOnly.FromDateTime(date.AddYears(10));
			}
		}
      }
}
#endif