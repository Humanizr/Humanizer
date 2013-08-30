﻿using System.Diagnostics;

using Humanizer.Properties;

namespace Humanizer.Configuration
{
    class RomanianFormatter : DefaultFormatter
    {
        public override string DateHumanize__years_ago(int numberOfYears)
        {
            Debug.Assert(numberOfYears > 1);

            if (0 < numberOfYears % 100 && numberOfYears % 100 < 20)
            {
                return base.DateHumanize__years_ago(numberOfYears);
            }

            return string.Format(Resources.DateHumanize__years_ago_above_20, numberOfYears);
        }

        public override string DateHumanize__days_ago(int numberOfDays)
        {
            Debug.Assert(numberOfDays > 1);

            if (0 < numberOfDays % 100 && numberOfDays % 100 < 20)
            {
                return base.DateHumanize__days_ago(numberOfDays);
            }

            return string.Format(Resources.DateHumanize__days_ago_above_20, numberOfDays);
        }

        public override string DateHumanize__hours_ago(int numberOfHours)
        {
            Debug.Assert(numberOfHours > 1);

            if (0 < numberOfHours%100 && numberOfHours%100 < 20)
            {
                return base.DateHumanize__hours_ago(numberOfHours);
            }

            return string.Format(Resources.DateHumanize__hours_ago_above_20, numberOfHours);
        }

        public override string DateHumanize__minutes_ago(int numberOfMinutes)
        {
            Debug.Assert(numberOfMinutes > 1);

            if (0 < numberOfMinutes % 100 && numberOfMinutes % 100 < 20)
            {
                return base.DateHumanize__minutes_ago(numberOfMinutes);
            }

            return string.Format(Resources.DateHumanize__minutes_ago_above_20, numberOfMinutes);
        }

        public override string DateHumanize__seconds_ago(int numberOfSeconds)
        {
            Debug.Assert(numberOfSeconds > 1);

            if (0 < numberOfSeconds%100 && numberOfSeconds%100 < 20)
            {
                return base.DateHumanize__seconds_ago(numberOfSeconds);
            }

            return string.Format(Resources.DateHumanize__seconds_ago_above_20, numberOfSeconds);
        }
    }
}