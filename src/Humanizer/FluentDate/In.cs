using System;
namespace Humanizer
{
    public partial class In
    {
        public static DateTime TheYear(int year)
        {
            return new DateTime(year, 1, 1);
        }
    }
}
