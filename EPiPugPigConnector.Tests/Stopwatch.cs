﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPiPugPigConnector.Tests
{
    public class Stopwatch
    {
        private System.Diagnostics.Stopwatch stopwatch = null;

        public Stopwatch()
        {
            stopwatch.Start();
        }

        public string Stop()
        {
            stopwatch.Stop();

            string result = string.Format("{0} min, {1} s, {2} ms",
                stopwatch.Elapsed.Minutes,
                stopwatch.Elapsed.Seconds,
                stopwatch.Elapsed.Milliseconds);
                //stopwatch.Elapsed.Ticks,

            System.Diagnostics.Debug.WriteLine(result);
            return result;
        }
    }
}