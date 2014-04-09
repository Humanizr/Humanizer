using System;
using Humanizer.Configuration;
using Humanizer.Localisation;

namespace Humanizer.DateTimeStrategy
{
    public class PrecisionDateTimeStrategy : IDateTimeHumanizeStrategy
    {
        private readonly double _precision;

        public PrecisionDateTimeStrategy(double precision = .75)
        {
            _precision = precision;
        }

        public string Humanize(DateTime input, DateTime comparisonBase)
        {
            var ts = new TimeSpan(Math.Abs(comparisonBase.Ticks - input.Ticks));
            var tense = input > comparisonBase ? Tense.Future : Tense.Past;

            if (ts.TotalMilliseconds < 1000 * _precision)
                return Configurator.Formatter.DateHumanize(TimeUnit.Millisecond, tense, 0);

            if (ts.TotalSeconds < 60 * _precision)
                return Configurator.Formatter.DateHumanize(TimeUnit.Second, tense, ts.Seconds);

            if (ts.TotalSeconds < 120 * _precision)
                return Configurator.Formatter.DateHumanize(TimeUnit.Minute, tense, 1);

            if (ts.TotalMinutes < 60 * _precision)
                return Configurator.Formatter.DateHumanize(TimeUnit.Minute, tense, ts.Minutes);

            if (ts.TotalMinutes < 120 * _precision)
                return Configurator.Formatter.DateHumanize(TimeUnit.Hour, tense, 1);

            if (ts.TotalHours < 24 * _precision)
                return Configurator.Formatter.DateHumanize(TimeUnit.Hour, tense, ts.Hours);

            if (ts.TotalHours < 48 * _precision)
                return Configurator.Formatter.DateHumanize(TimeUnit.Day, tense, 1);

            if (ts.TotalDays < 30 * _precision)
                return Configurator.Formatter.DateHumanize(TimeUnit.Day, tense, ts.Days);

            if (ts.TotalDays < 60 * _precision)
                return Configurator.Formatter.DateHumanize(TimeUnit.Month, tense, 1);

            if (ts.TotalDays < 365 * _precision)
            {
                int months = Convert.ToInt32(Math.Floor(ts.TotalDays / 29.5));
                return Configurator.Formatter.DateHumanize(TimeUnit.Month, tense, months);
            }

            int years = Convert.ToInt32(Math.Floor(ts.TotalDays / 365));
            if (years == 0) 
                years = 1;

            return Configurator.Formatter.DateHumanize(TimeUnit.Year, tense, years);
        }
    }
}