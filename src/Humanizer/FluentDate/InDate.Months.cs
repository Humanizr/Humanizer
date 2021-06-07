 
#if NET6_0_OR_GREATER

using System;

namespace Humanizer
{
    public partial class InDate
    {
   
        /// <summary>
        /// Returns 1st of January of the current year
        /// </summary>
		public static DateOnly January
		{
			get { return new DateOnly(DateTime.UtcNow.Year, 1, 1); }
		}

        /// <summary>
        /// Returns 1st of January of the year passed in
        /// </summary>
        public static DateOnly JanuaryOf(int year)
		{
			return new DateOnly(year, 1, 1);
		}
     
        /// <summary>
        /// Returns 1st of February of the current year
        /// </summary>
		public static DateOnly February
		{
			get { return new DateOnly(DateTime.UtcNow.Year, 2, 1); }
		}

        /// <summary>
        /// Returns 1st of February of the year passed in
        /// </summary>
        public static DateOnly FebruaryOf(int year)
		{
			return new DateOnly(year, 2, 1);
		}
     
        /// <summary>
        /// Returns 1st of March of the current year
        /// </summary>
		public static DateOnly March
		{
			get { return new DateOnly(DateTime.UtcNow.Year, 3, 1); }
		}

        /// <summary>
        /// Returns 1st of March of the year passed in
        /// </summary>
        public static DateOnly MarchOf(int year)
		{
			return new DateOnly(year, 3, 1);
		}
     
        /// <summary>
        /// Returns 1st of April of the current year
        /// </summary>
		public static DateOnly April
		{
			get { return new DateOnly(DateTime.UtcNow.Year, 4, 1); }
		}

        /// <summary>
        /// Returns 1st of April of the year passed in
        /// </summary>
        public static DateOnly AprilOf(int year)
		{
			return new DateOnly(year, 4, 1);
		}
     
        /// <summary>
        /// Returns 1st of May of the current year
        /// </summary>
		public static DateOnly May
		{
			get { return new DateOnly(DateTime.UtcNow.Year, 5, 1); }
		}

        /// <summary>
        /// Returns 1st of May of the year passed in
        /// </summary>
        public static DateOnly MayOf(int year)
		{
			return new DateOnly(year, 5, 1);
		}
     
        /// <summary>
        /// Returns 1st of June of the current year
        /// </summary>
		public static DateOnly June
		{
			get { return new DateOnly(DateTime.UtcNow.Year, 6, 1); }
		}

        /// <summary>
        /// Returns 1st of June of the year passed in
        /// </summary>
        public static DateOnly JuneOf(int year)
		{
			return new DateOnly(year, 6, 1);
		}
     
        /// <summary>
        /// Returns 1st of July of the current year
        /// </summary>
		public static DateOnly July
		{
			get { return new DateOnly(DateTime.UtcNow.Year, 7, 1); }
		}

        /// <summary>
        /// Returns 1st of July of the year passed in
        /// </summary>
        public static DateOnly JulyOf(int year)
		{
			return new DateOnly(year, 7, 1);
		}
     
        /// <summary>
        /// Returns 1st of August of the current year
        /// </summary>
		public static DateOnly August
		{
			get { return new DateOnly(DateTime.UtcNow.Year, 8, 1); }
		}

        /// <summary>
        /// Returns 1st of August of the year passed in
        /// </summary>
        public static DateOnly AugustOf(int year)
		{
			return new DateOnly(year, 8, 1);
		}
     
        /// <summary>
        /// Returns 1st of September of the current year
        /// </summary>
		public static DateOnly September
		{
			get { return new DateOnly(DateTime.UtcNow.Year, 9, 1); }
		}

        /// <summary>
        /// Returns 1st of September of the year passed in
        /// </summary>
        public static DateOnly SeptemberOf(int year)
		{
			return new DateOnly(year, 9, 1);
		}
     
        /// <summary>
        /// Returns 1st of October of the current year
        /// </summary>
		public static DateOnly October
		{
			get { return new DateOnly(DateTime.UtcNow.Year, 10, 1); }
		}

        /// <summary>
        /// Returns 1st of October of the year passed in
        /// </summary>
        public static DateOnly OctoberOf(int year)
		{
			return new DateOnly(year, 10, 1);
		}
     
        /// <summary>
        /// Returns 1st of November of the current year
        /// </summary>
		public static DateOnly November
		{
			get { return new DateOnly(DateTime.UtcNow.Year, 11, 1); }
		}

        /// <summary>
        /// Returns 1st of November of the year passed in
        /// </summary>
        public static DateOnly NovemberOf(int year)
		{
			return new DateOnly(year, 11, 1);
		}
     
        /// <summary>
        /// Returns 1st of December of the current year
        /// </summary>
		public static DateOnly December
		{
			get { return new DateOnly(DateTime.UtcNow.Year, 12, 1); }
		}

        /// <summary>
        /// Returns 1st of December of the year passed in
        /// </summary>
        public static DateOnly DecemberOf(int year)
		{
			return new DateOnly(year, 12, 1);
		}
      }
}
#endif