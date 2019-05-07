using System;
using System.Numerics;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using Xunit;
using Xunit.Abstractions;

namespace DotNetBa.Internals.SpansAndSuch.Spans
{
    public class SpanPerformance
    {
        private readonly ITestOutputHelper _helper;

        public SpanPerformance(ITestOutputHelper helper)
        {
            _helper = helper;
        }

        [Fact]
        public void StringSlicing()
        {
            var manualConfig = ManualConfig.Create(DefaultConfig.Instance)
                .With(loggers: new XunitTestOutputLogger(_helper));

            BenchmarkRunner.Run<StringSlicingBenchmark>(manualConfig);
        }

        [Fact]
        public void DataCoercion()
        {
            var manualConfig = ManualConfig.Create(DefaultConfig.Instance)
                .With(loggers: new XunitTestOutputLogger(_helper));

            BenchmarkRunner.Run<CoercionBenchmark>(manualConfig);
        }

        [Fact]
        public void SimdCoercion()
        {
            var manualConfig = ManualConfig.Create(DefaultConfig.Instance)
                .With(loggers: new XunitTestOutputLogger(_helper));

            BenchmarkRunner.Run<SimdCoercionBenchmark>(manualConfig);
        }

        [MemoryDiagnoser]
        public class StringSlicingBenchmark
        {
            private string _data;

            [GlobalSetup]
            public void Setup()
            {
                _data = new string('a', 10000);
            }

            [Benchmark(Baseline = true)]
            public void SumSubstrings()
            {
                long total = 0;

                for (var i = 0; i < _data.Length / 3; i++)
                {
                    var substringSum = SubstringSum(_data.Substring(i * 3, 3));
                    total += substringSum;
                }

                Console.WriteLine(total);
            }

            [Benchmark]
            public void SumSubspan()
            {
                long total = 0;

                for (var i = 0; i < _data.Length / 3; i++)
                {
                    var substringSum = SubstringSum(_data.AsSpan().Slice(i * 3, 3));
                    total += substringSum;
                }

                Console.WriteLine(total);
            }

            private int SubstringSum(string s)
            {
                return s[0] + s[1] + s[2];
            }

            private int SubstringSum(ReadOnlySpan<char> s)
            {
                return s[0] + s[1] + s[2];
            }
        }

        [MemoryDiagnoser]
        public class CoercionBenchmark
        {
            private byte[] _data;

            private Span<byte> DataSpan => _data.AsSpan();

            [GlobalSetup]
            public void Setup()
            {
                _data = new byte[100000];
                var r = new Random();

                for (var i = 0; i < _data.Length; i++)
                {
                    _data[i] = (byte) r.Next(0, 256);
                }
            }

            [Benchmark(Baseline = true)]
            public void NonSpanMax()
            {
                var ints = new int[_data.Length / sizeof(int)];
                Buffer.BlockCopy(_data, 0, ints, 0, _data.Length);

                var max = 0;

                foreach (var i in ints)
                {
                    if (max < i)
                    {
                        max = i;
                    }
                }
            }

            [Benchmark]
            public void SpanMax()
            {
                var span = MemoryMarshal.Cast<byte, int>(DataSpan);

                var max = 0;

                foreach (var i in span)
                {
                    if (max < i)
                    {
                        max = i;
                    }
                }
            }
        }

        [MemoryDiagnoser]
        public class SimdCoercionBenchmark
        {
            private byte[] _data;

            private Span<byte> DataSpan => _data.AsSpan();

            [GlobalSetup]
            public void Setup()
            {
                _data = new byte[131072];
                var r = new Random();

                for (var i = 0; i < _data.Length; i++)
                {
                    _data[i] = (byte) r.Next(0, 32);
                }
            }

            [Benchmark(Baseline = true)]
            public void IntSum()
            {
                var total = 0L;

                var span = MemoryMarshal.Cast<byte, int>(DataSpan);

                foreach (var i in span)
                {
                    total += i;
                }

                Console.WriteLine(total);
            }

            [Benchmark]
            public void VectorSum()
            {
                var span = MemoryMarshal.Cast<byte, Vector<int>>(DataSpan);

                var totalVector = Vector<int>.Zero;

                foreach (var vector in span)
                {
                    totalVector = Vector.Add(totalVector, vector);
                }

                Console.WriteLine(Vector.Dot(totalVector, Vector<int>.One));
            }
        }
    }
}
