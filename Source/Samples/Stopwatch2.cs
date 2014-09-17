using System;
using System.Diagnostics;

namespace Miracle.FileZilla.Api.Samples
{
    internal class Stopwatch2 : Stopwatch
    {
        private TimeSpan? _lastSplit;

        public new static Stopwatch2 StartNew()
        {
            var stopwatch2 = new Stopwatch2();
            stopwatch2.Start();
            return stopwatch2;
        }

        public TimeSpan GetDelta()
        {
            var elapsed = this.Elapsed;
            var delta = elapsed - _lastSplit.GetValueOrDefault();
            _lastSplit = elapsed;
            return delta;
        }
    }
}