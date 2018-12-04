using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using Utils.Data;

namespace Utils.Debuging
{
    public class StopWatcher
    {
        public ReactiveProperty<Tuple<string, float>> OnTimerFlushed = new ReactiveProperty<Tuple<string, float>>();

        public Dictionary<string, Stopwatch> Timers = new Dictionary<string, Stopwatch>();
        public Dictionary<string, RingBuffer<long>> Buffers = new Dictionary<string, RingBuffer<long>>();

        public void InitializeTimers(string[] TimerNames, int bufferLength)
        {
            foreach (var name in TimerNames)
            {
                Timers.Add(name, new Stopwatch());
                Buffers.Add(name, new RingBuffer<long>(bufferLength));
            }
        }

        public void StartTimer(string name)
        {
            Stopwatch timer;
            if (Timers.TryGetValue(name, out timer))
            {
                timer.Reset();
                timer.Start();
            }
        }

        public void StopTimer(string name)
        {
            Stopwatch timer;
            if (Timers.TryGetValue(name, out timer))
            {
                timer.Stop();
            }
        }

        public void SafeTimeToBuffer(string name)
        {
            Stopwatch timer;
            RingBuffer<long> buffer;
            if (Timers.TryGetValue(name, out timer) && Buffers.TryGetValue(name, out buffer))
            {
                buffer.Add(timer.ElapsedMilliseconds);
            }
        }

        public void FlushTimer(string name, bool fromBuffer = false)
        {
            if (!fromBuffer)
            {
                Stopwatch timer;
                if (!Timers.TryGetValue(name, out timer)) return;

                OnTimerFlushed.Value = new Tuple<string, float>(name, timer.ElapsedMilliseconds);
            }
            else
            {
                RingBuffer<long> buffer;
                if (!Buffers.TryGetValue(name, out buffer)) return;

                long time = 0;
                for (var i = 0; i < buffer.Capacity; i++)
                {
                    time += buffer[i];
                }
                OnTimerFlushed.Value = new Tuple<string, float>(name, (float)time / buffer.Capacity);
            }
        }

        public float GetTime(string name, bool fromBuffer = false)
        {
            if (!fromBuffer)
            {
                Stopwatch timer;
                return !Timers.TryGetValue(name, out timer) ? 0 : timer.ElapsedMilliseconds;
            }
            RingBuffer<long> buffer;
            if (!Buffers.TryGetValue(name, out buffer)) return 0;

            long time = 0;
            for (var i = 0; i < buffer.Capacity; i++)
            {
                time += buffer[i];
            }
            return (float)time / buffer.Capacity;
        }

        public void FlushAll(bool fromBuffer)
        {
            foreach (var name in Timers.Keys)
            {
                FlushTimer(name, fromBuffer);
            }
        }
    }
}
