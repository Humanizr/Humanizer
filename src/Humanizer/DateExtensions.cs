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
using System.ComponentModel;

namespace Humanizer
{
    [Localizable(true)]
    public static class DateExtensions
    {
        // http://stackoverflow.com/questions/11/how-do-i-calculate-relative-time
        public static string Humanize(this DateTime input, bool utcDate = true, DateTime? now = null)
        {
            if (now == null)
            {
                now = DateTime.UtcNow;
            }
            const int second = 1;
            const int minute = 60 * second;
            const int hour = 60 * minute;
            const int day = 24 * hour;
            const int month = 30 * day;

            var comparisonBase = now.Value;
            if (!utcDate)
                comparisonBase = comparisonBase.ToLocalTime();

            if (input > comparisonBase)
                return Resources.DateExtensions_FutureDate_not_yet;

            var ts = new TimeSpan(comparisonBase.Ticks - input.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * minute)
                return ts.Seconds == 1 ? Resources.DateExtensions_OneSecondAgo_one_second_ago : string.Format(Resources.DateExtensions_SecondsAgo__seconds_ago, ts.Seconds);

            if (delta < 2 * minute)
                return Resources.DateExtensions_OneMinuteAgo_a_minute_ago;

            if (delta < 45 * minute)
                return string.Format(Resources.DateExtensions_MinutesAgo__minutes_ago, ts.Minutes);

            if (delta < 90 * minute)
                return Resources.DateExtensions_OneHourAgo_an_hour_ago;

            if (delta < 24 * hour)
                return string.Format(Resources.DateExtensions_HoursAgo__hours_ago, ts.Hours);

            if (delta < 48 * hour)
                return Resources.DateExtensions_Yesterday_yesterday;

            if (delta < 30 * day)
                return string.Format(Resources.DateExtensions_DaysAgo__days_ago, ts.Days);

            if (delta < 12 * month)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? Resources.DateExtensions_OneMonthAgo_one_month_ago : string.Format(Resources.DateExtensions_MonthsAgo__months_ago, months);
            }

            int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
            return years <= 1 ? Resources.DateExtensions_OneYearAgo_one_year_ago : string.Format(Resources.DateExtensions_YearsAgo__years_ago, years);
        }
    }
}