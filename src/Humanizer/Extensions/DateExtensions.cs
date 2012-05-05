// Copyright (C) 2012, Mehdi Khalili
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//     * Redistributions of source code must retain the above copyright
//       notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//       notice, this list of conditions and the following disclaimer in the
//       documentation and/or other materials provided with the distribution.
//     * Neither the name of the <organization> nor the
//       names of its contributors may be used to endorse or promote products
//       derived from this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.using System;

using System;

namespace Humanizer.Extensions
{
    public static class DateExtensions
    {
        public static string FutureDate = "not yet";
        public static string OneSecondAgo = "one second ago";
        public static string SecondsAgo = " seconds ago";
        public static string OneMinuteAgo = "a minute ago";
        public static string MinutesAgo = " minutes ago";
        public static string OneHourAgo = "an hour ago";
        public static string HoursAgo = " hours ago";
        public static string Yesterday = "yesterday";
        public static string DaysAgo = " days ago";
        public static string OneMonthAgo = "one month ago";
        public static string MonthsAgo = " months ago";
        public static string OneYearAgo = "one year ago";
        public static string YearsAgo = " years ago";

        // http://stackoverflow.com/questions/11/how-do-i-calculate-relative-time
        public static string Humanize(this DateTime input, bool utcDate = true)
        {
            const int second = 1;
            const int minute = 60 * second;
            const int hour = 60 * minute;
            const int day = 24 * hour;
            const int month = 30 * day;

            var comparisonBase = DateTime.UtcNow;
            if (!utcDate)
                comparisonBase = comparisonBase.ToLocalTime();

            if (input > comparisonBase)
                return FutureDate;

            var ts = new TimeSpan(comparisonBase.Ticks - input.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * minute)
                return ts.Seconds == 1 ? OneSecondAgo : ts.Seconds + SecondsAgo;

            if (delta < 2 * minute)
                return OneMinuteAgo;

            if (delta < 45 * minute)
                return ts.Minutes + MinutesAgo;

            if (delta < 90 * minute)
                return OneHourAgo;

            if (delta < 24 * hour)
                return ts.Hours + HoursAgo;

            if (delta < 48 * hour)
                return Yesterday;

            if (delta < 30 * day)
                return ts.Days + DaysAgo;

            if (delta < 12 * month)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? OneMonthAgo : months + MonthsAgo;
            }

            int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
            return years <= 1 ? OneYearAgo : years + YearsAgo;
        }
    }
}