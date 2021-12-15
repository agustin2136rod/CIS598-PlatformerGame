/* TimeHandler.cs
 * Written By: Agustin Rodriguez
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CollectTheCoins.Handlers
{
    /*
     * Class to represent the time left in the game and handles pausing 
     */
    public class TimeHandler
    {
        /// <summary>
        /// timer for the class
        /// </summary>
        private Stopwatch _timer;

        /// <summary>
        /// duration of the level
        /// </summary>
        private TimeSpan Duration;

        /// <summary>
        /// Constructor for the class
        /// </summary>
        /// <param name="duration">duration for the level</param>
        public TimeHandler(TimeSpan duration)
        {
            Duration = duration;
        }

        /// <summary>
        /// start the timer
        /// </summary>
        public void Start()
        {
            _timer = Stopwatch.StartNew();
        }

        /// <summary>
        /// set the duration of the level
        /// </summary>
        /// <param name="time"></param>
        public void SetDuration(TimeSpan time)
        {
            Duration = time;
        }

        /// <summary>
        /// pause the timer
        /// </summary>
        public void Pause()
        {
            if (_timer is null) throw new InvalidOperationException();
            if (_timer.IsRunning) _timer.Stop();
        }

        /// <summary>
        /// resume the timer
        /// </summary>
        public void Resume()
        {
            if (_timer is null) throw new InvalidOperationException();
            if (!_timer.IsRunning) _timer.Start();
        }

        /// <summary>
        /// get the remaining time in the level
        /// </summary>
        public TimeSpan RemainingTime
        {
            get
            {
                if (_timer is null) return TimeSpan.MaxValue;

                TimeSpan remainingTime = Duration - _timer.Elapsed;
                return remainingTime > TimeSpan.Zero ? remainingTime : TimeSpan.Zero;
            }
        }
    }
}
