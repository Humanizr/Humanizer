using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Humanizer.Bytes
{
    public class ByteRate
    {
        public ByteSize size { get; private set;}

        public TimeSpan interval { get; private set; }

        public ByteRate(ByteSize size, TimeSpan interval)
        {
            this.size = size;
            this.interval = interval;
        }

        /// <summary>
        /// Return this rate as a string, EG "2 MB/s"
        /// </summary>
        /// <returns></returns>
        public string ToRatePerSecond()
        {
            return (new ByteSize(size.Bytes / interval.TotalSeconds)).Humanize() + "/s";
        }
    }
}
