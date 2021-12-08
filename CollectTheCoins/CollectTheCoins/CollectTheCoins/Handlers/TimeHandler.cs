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
        private Stopwatch _timer;

        private TimeSpan Duration;

        public TimeHandler(TimeSpan duration)
        {
            Duration = duration;
        }

        public void Start()
        {
            _timer = Stopwatch.StartNew();
        }

        public void SetDuration(TimeSpan time)
        {
            Duration = time;
        }

        public void Pause()
        {
            if (_timer is null) throw new InvalidOperationException();
            if (_timer.IsRunning) _timer.Stop();
        }

        public void Resume()
        {
            if (_timer is null) throw new InvalidOperationException();
            if (!_timer.IsRunning) _timer.Start();
        }

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
