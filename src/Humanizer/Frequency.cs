using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Humanizer.Localisation;

namespace Humanizer
{
    public class Frequency
    {
        private DateTime _startDate = DateTime.MinValue;
        private DateTime _endDate = DateTime.MinValue;
        private Frequencies _occurrenceFrequency;
        private List<DateTime> _listOfOccurrences;
        private TimeSpan _timeSpanBetweenOccurrences;
        private NotStandardFrequencyInfo _notStandardFrequencyInfo;

        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                OnBoundaryDateChanged();
            }
        }
        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                OnBoundaryDateChanged();
            }
        }
        public Frequencies OccurrenceFrequency
        {
            get { return _occurrenceFrequency; }
        }
        public List<DateTime> ListOfOccurrences
        {
            get { return _listOfOccurrences ?? (_listOfOccurrences = new List<DateTime>()); }
        }
        public NotStandardFrequencyInfo NotStandardFrequencyInfo
        {
            get { return _notStandardFrequencyInfo; }
            set { _notStandardFrequencyInfo = value; }
        }

        /// <summary>
        /// Constructor of Frequency for an event that occurs at most once
        /// a year, at the given date
        /// </summary>
        /// <param name="occurrence">Date at which the event occurs</param>
        public Frequency(DateTime occurrence)
        {
            _listOfOccurrences = new List<DateTime>() { occurrence };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timespanBetweenOccurrences">Time elapsed between two occurrences</param>
        private Frequency(TimeSpan timespanBetweenOccurrences)
        {
            _occurrenceFrequency = Frequencies.NotStandard;
            _timeSpanBetweenOccurrences = timespanBetweenOccurrences;
            SetupFrequencyFromTimeSpan(timespanBetweenOccurrences);
        }
        private Frequency()
        {   
        }

        public static Frequency Every(DateTime dateOfOccurrences)
        {
            return new Frequency(dateOfOccurrences)
                {
                    _occurrenceFrequency = Frequencies.Yearly
                };
        }
        public static Frequency Every(TimeSpan timespanBetweenOccurrences)
        {
            return new Frequency(timespanBetweenOccurrences) { _occurrenceFrequency = Frequencies.NotStandard };
        }
        public static Frequency Every()
        {
            return new Frequency();
        }

        public Frequency Month()
        {
            _timeSpanBetweenOccurrences = new TimeSpan(30, 0, 0, 0, 0);
            _occurrenceFrequency = Frequencies.Monthly;
            return this;
        }
        public Frequency Day()
        {
            _timeSpanBetweenOccurrences = new TimeSpan(1, 0, 0, 0, 0);
            _occurrenceFrequency = Frequencies.Daily;
            return this;
        }
        public Frequency Year()
        {
            _timeSpanBetweenOccurrences = new TimeSpan(365, 0, 0, 0, 0);
            _occurrenceFrequency = Frequencies.Yearly;
            return this;
        }
        public Frequency Minute()
        {
            _timeSpanBetweenOccurrences = new TimeSpan(0, 0, 1, 0, 0);
            _occurrenceFrequency = Frequencies.EveryMinute;
            return this;
        }
        public Frequency Hour()
        {
            _timeSpanBetweenOccurrences = new TimeSpan(0, 1, 0, 0, 0);
            _occurrenceFrequency = Frequencies.Hourly;
            return this;
        }
        public Frequency Week()
        {
            _timeSpanBetweenOccurrences = new TimeSpan(7, 0, 0, 0, 0);
            _occurrenceFrequency = Frequencies.Weekly;
            return this;
        }
        public Frequency Quarter()
        {
            _timeSpanBetweenOccurrences = new TimeSpan(90, 0, 0, 0, 0);
            _occurrenceFrequency = Frequencies.Quarterly;
            return this;
        }

        public Frequency StartingOn(DateTime startingDate)
        {
            StartDate = startingDate;
            return this;
            
        }
        public Frequency EndingOn(DateTime endingDate)
        {
            EndDate = endingDate;
            return this;
        }

        /// <summary>
        /// Recomputes the parameters of the object, every time that a modification is brought
        /// </summary>
        private void OnBoundaryDateChanged()
        {
            if (DateTime.Compare(_endDate,DateTime.MinValue) != 0)
            {
                // If the list of occurrences is empty, it means we know the timespan
                // between each occurrence. So we will use it to build the list, along with
                // the start date and the end date (to make sur we stay in the right range).
                if ((_listOfOccurrences == null || _listOfOccurrences.Count == 0) &&
                        (_startDate != DateTime.MinValue && _endDate != DateTime.MinValue) &&
                        DateTime.Compare(_startDate, _endDate) < 0)
                {
                    ListOfOccurrences.Add(_startDate);
                }

                // If the user sets an end date bafore the start date, it doesn't make sense --> the
                // event will simply never happen
                if (DateTime.Compare(_startDate, _endDate) > 0)
	            {
                    _occurrenceFrequency = Frequencies.Never;
                    return;
	            }

                // If the periods covers less than one year, we have to check whether the event may happens
                // once or not at all
                if (_endDate - _startDate <= new TimeSpan(365,0,0,0))
                {
                    if (((_startDate.Year == _endDate.Year) && (DateTime.Compare(_startDate, ListOfOccurrences[0]) < 0) && (DateTime.Compare(_endDate, ListOfOccurrences[0]) > 0)) ||
                                ((_startDate.Year < _endDate.Year) &&
                                (DateTime.Compare(_startDate,new DateTime(_startDate.Year, ListOfOccurrences[0].Month, ListOfOccurrences[0].Day)) < 0) &&
                                (DateTime.Compare(_endDate, new DateTime(_endDate.Year, ListOfOccurrences[0].Month, ListOfOccurrences[0].Day)) >= 0)))
                    {
                        _occurrenceFrequency = Frequencies.OnceOff;
                    }
                    else
                    {
                        _occurrenceFrequency = Frequencies.Never;
                    }
                }   
            }
        }
        private void SetupFrequencyFromTimeSpan(TimeSpan input)
        {
            NotStandardFrequencyInfo = new NotStandardFrequencyInfo();
            if ((int)input.TotalDays > 364)
            {
                NotStandardFrequencyInfo.Frequency = new KeyValuePair<TimeUnit, double>(TimeUnit.Year, SplitTimespan(input, 365, (int)input.TotalDays));
            }
            else if ((int)input.TotalDays > 29)
            {
                NotStandardFrequencyInfo.Frequency = new KeyValuePair<TimeUnit, double>(TimeUnit.Month, SplitTimespan(input, 30, (int)input.TotalDays));
            }
            else if ((int)input.TotalDays > 0)
            {
                NotStandardFrequencyInfo.Frequency = new KeyValuePair<TimeUnit, double>(TimeUnit.Day, SplitTimespan(input, 24, (int)input.TotalHours));
            }
            else if ((int)input.TotalHours > 0)
	        {
                NotStandardFrequencyInfo.Frequency = new KeyValuePair<TimeUnit, double>(TimeUnit.Hour, SplitTimespan(input, 60, (int)input.TotalMinutes));
	        }
            else if ((int)input.TotalMinutes > 0)
	        {
                NotStandardFrequencyInfo.Frequency = new KeyValuePair<TimeUnit, double>(TimeUnit.Minute, SplitTimespan(input, 60, (int)input.TotalSeconds));
            }
            else if ((int)input.TotalSeconds > 0)
	        {
                NotStandardFrequencyInfo.Frequency = new KeyValuePair<TimeUnit, double>(TimeUnit.Second, SplitTimespan(input, 1000, (int)input.TotalMilliseconds));
            }
        }

        private double SplitTimespan(TimeSpan input, int nbUnit, int referenceAmount)
        {
            int intPart = referenceAmount / nbUnit;
            double durationAsDouble = (double)intPart;
            int remainder = referenceAmount - intPart * nbUnit;
            if (remainder == 0)
            {
                _occurrenceFrequency = Frequencies.Yearly;
                return durationAsDouble;
            }
            if (remainder > 3 * nbUnit / 4)
            {
                durationAsDouble += 1.0;
            }
            if (((3 * nbUnit / 4) > remainder) && remainder > (nbUnit / 4))
            {
                durationAsDouble += 0.5; 
            }
            return durationAsDouble;
        }

    }
}
