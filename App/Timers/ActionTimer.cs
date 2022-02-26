using System.Diagnostics;

namespace SerializationDataLib.Timers
{
    internal class ActionTimer
    {
        public int IterationsCount { get; init; }

        private Stopwatch _timer;

        public ActionTimer() : this(1) { }
        public ActionTimer(int iterationsCount)
        {
            if (iterationsCount <= 0)
                throw new ArgumentOutOfRangeException($"{nameof(iterationsCount)} cannot be less than 1.");

            IterationsCount = iterationsCount;
            _timer = new Stopwatch();
        }

        public TimeSpan MeasureTimeForAction<T1, T2>(RefAction<T1, T2> action, T1 source, ref T2 target)
        {
            TimeSpan result = default;
            // Todo: correct measures for multiple iterations.
            for (int i = 0; i < IterationsCount; ++i)
            {
                _timer.Start();
                action(source, ref target);
                _timer.Stop();
                result += _timer.Elapsed;
                _timer.Reset();
            }

            return result / IterationsCount;
        }
    }
}
