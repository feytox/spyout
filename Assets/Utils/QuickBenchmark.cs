using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class QuickBenchmark
    {
        public const double DefaultWarmup = 0.1; // in seconds
        public const double DefaultTimeout = 0.4; // in seconds

        public static BenchResult Bench<T>(
            Func<T> func,
            double warmup = DefaultWarmup,
            double timeout = DefaultTimeout
        )
        {
            // warmup
            var start = Time.realtimeSinceStartupAsDouble;
            while (Time.realtimeSinceStartupAsDouble - start < warmup)
                func();

            // benchmark
            var benches = new List<double>();
            start = Time.realtimeSinceStartupAsDouble;
            while (Time.realtimeSinceStartupAsDouble - start < timeout)
                benches.Add(BenchOnce(func));

            return GetStats(benches);
        }

        public static double BenchOnce<T>(Func<T> func)
        {
            var start = Time.realtimeSinceStartupAsDouble;
            func();
            return Time.realtimeSinceStartupAsDouble - start;
        }

        public struct BenchResult
        {
            public double avg;
            public double median;
            public double min;
            public double max;

            public readonly void Print(string benchName) =>
                Debug.Log(
                    $"{benchName}: avg={avg:F10}, median={median:F10}, min={min:F10}, max={max:F10}"
                );
        }

        private static BenchResult GetStats(List<double> benches)
        {
            var sum = 0.0;
            var min = double.MaxValue;
            var max = double.MinValue;

            benches.Sort();
            for (var i = 0; i < benches.Count; i++)
            {
                sum += benches[i];
                min = Math.Min(min, benches[i]);
                max = Math.Max(max, benches[i]);
            }

            return new BenchResult()
            {
                avg = sum / benches.Count,
                median = benches[benches.Count / 2],
                min = min,
                max = max,
            };
        }
    }
}
