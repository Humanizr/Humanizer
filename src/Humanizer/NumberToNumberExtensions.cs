using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Humanizer
{
    /// <summary>
    /// Number to Number extensions
    /// </summary>
    public static class NumberToNumberExtensions
    {
        /// <summary>
        /// 5.Tens == 50
        /// </summary>
        public static int Tens(this int input)
        {
            return input*10;
        }

        /// <summary>
        /// 5.Tens == 50
        /// </summary>
        public static uint Tens(this uint input)
        {
            return input*10;
        }

        /// <summary>
        /// 5.Tens == 50
        /// </summary>
        public static long Tens(this long input)
        {
            return input*10;
        }

        /// <summary>
        /// 5.Tens == 50
        /// </summary>
        public static ulong Tens(this ulong input)
        {
            return input*10;
        }

        /// <summary>
        /// 5.Tens == 50
        /// </summary>
        public static double Tens(this double input)
        {
            return input*10;
        }

        /// <summary>
        /// 4.Hundreds() == 400
        /// </summary>
        public static int Hundreds(this int input)
        {
            return input*100;
        }

        /// <summary>
        /// 4.Hundreds() == 400
        /// </summary>
        public static uint Hundreds(this uint input)
        {
            return input*100;
        }

        /// <summary>
        /// 4.Hundreds() == 400
        /// </summary>
        public static long Hundreds(this long input)
        {
            return input*100;
        }

        /// <summary>
        /// 4.Hundreds() == 400
        /// </summary>
        public static ulong Hundreds(this ulong input)
        {
            return input*100;
        }

        /// <summary>
        /// 4.Hundreds() == 400
        /// </summary>
        public static double Hundreds(this double input)
        {
            return input*100;
        }

        /// <summary>
        /// 3.Thousands() == 3000
        /// </summary>
        public static int Thousands(this int input)
        {
            return input*1000;
        }

        /// <summary>
        /// 3.Thousands() == 3000
        /// </summary>
        public static uint Thousands(this uint input)
        {
            return input*1000;
        }

        /// <summary>
        /// 3.Thousands() == 3000
        /// </summary>
        public static long Thousands(this long input)
        {
            return input*1000;
        }

        /// <summary>
        /// 3.Thousands() == 3000
        /// </summary>
        public static ulong Thousands(this ulong input)
        {
            return input*1000;
        }

        /// <summary>
        /// 3.Thousands() == 3000
        /// </summary>
        public static double Thousands(this double input)
        {
            return input*1000;
        }

        /// <summary>
        /// 2.Millions() == 2000000
        /// </summary>
        public static int Millions(this int input)
        {
            return input*1000000;
        }

        /// <summary>
        /// 2.Millions() == 2000000
        /// </summary>
        public static uint Millions(this uint input)
        {
            return input*1000000;
        }

        /// <summary>
        /// 2.Millions() == 2000000
        /// </summary>
        public static long Millions(this long input)
        {
            return input*1000000;
        }

        /// <summary>
        /// 2.Millions() == 2000000
        /// </summary>
        public static ulong Millions(this ulong input)
        {
            return input*1000000;
        }

        /// <summary>
        /// 2.Millions() == 2000000
        /// </summary>
        public static double Millions(this double input)
        {
            return input*1000000;
        }

        /// <summary>
        /// 1.Billions() == 1000000000
        /// </summary>
        public static int Billions(this int input)
        {
            return input*1000000000;
        }

        /// <summary>
        /// 1.Billions() == 1000000000
        /// </summary>
        public static uint Billions(this uint input)
        {
            return input*1000000000;
        }

        /// <summary>
        /// 1.Billions() == 1000000000
        /// </summary>
        public static long Billions(this long input)
        {
            return input*1000000000;
        }

        /// <summary>
        /// 1.Billions() == 1000000000
        /// </summary>
        public static ulong Billions(this ulong input)
        {
            return input*1000000000;
        }

        /// <summary>
        /// 1.Billions() == 1000000000
        /// </summary>
        public static double Billions(this double input)
        {
            return input*1000000000;
        }
    }
}
