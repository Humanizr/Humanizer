using Humanizer.Localisation;

namespace Humanizer.DistanceOfTimeCalculators
{
    /// <summary>
    /// This structure is used to express the distance of time calculation
    /// </summary>
    public class DistanceOfTime
    {
        public int Count { get; set; }
        public TimeUnit Unit { get; set; }
        public TimeUnitTense Tense { get; set; }
        public TimeStructure Structure { get; set; }

        public static DistanceOfTime Create(
            TimeUnit timeUnit,
            TimeUnitTense timeUnitTense = TimeUnitTense.Past,
            TimeStructure timeStructure = TimeStructure.DateTime,
            int count = 1)
        {
            return new DistanceOfTime {
                Count = count,
                Structure = timeStructure,
                Tense = timeUnitTense,
                Unit = timeUnit
            };
        }
    }
}