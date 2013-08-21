using System;
namespace Humanizer
{
    public partial class In
    {
        /// <summary>
        /// Returns the first of January of the provided year
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static DateTime TheYear(int year)
        {
            return new DateTime(year, 1, 1);
        }
    }
}
