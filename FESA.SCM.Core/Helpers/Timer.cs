using System;
using System.Threading.Tasks;

namespace FESA.SCM.Core.Helpers
{
    public class Timer
    {
        private bool _timerRunning;
        private readonly TimeSpan _interval;
        private readonly Action _tick;
        private readonly bool _runOnce;

        public Timer(TimeSpan interval, Action tick, bool runOnce = false)
        {
            _interval = interval;
            _tick = tick;
            _runOnce = runOnce;
        }

        public Timer Start()
        {
            if (_timerRunning) return this;
            _timerRunning = true;
            RunTimer();

            return this;
        }

        public void Stop()
        {
            _timerRunning = false;
        }

        private async void RunTimer()
        {
            while (_timerRunning)
            {
                await Task.Delay(_interval);

                if (!_timerRunning) continue;
                _tick();

                if (_runOnce)
                {
                    Stop();
                }
            }
        }
    }
}