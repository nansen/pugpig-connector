using System.Diagnostics;

namespace EPiPugPigConnector.Utils
{
    public class StopwatchTimer
    {
        private System.Diagnostics.Stopwatch stopwatch;

        /// <summary>
        /// Initializes a new System.Diagnostics.Stopwatch and starts it.
        /// </summary>
        public StopwatchTimer()
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();
        }

        /// <summary>
        /// Stops the stopwatch
        /// </summary>
        /// <returns>Returns the time elapsed in min, s, ms value string.</returns>
        public string Stop()
        {
            stopwatch.Stop();

            string result = string.Format("{0}min {1}s {2}ms",
                stopwatch.Elapsed.Minutes,
                stopwatch.Elapsed.Seconds,
                stopwatch.Elapsed.Milliseconds);
                //stopwatch.Elapsed.Ticks,

            System.Diagnostics.Debug.WriteLine(result);
            return result;
        }
    }
}
