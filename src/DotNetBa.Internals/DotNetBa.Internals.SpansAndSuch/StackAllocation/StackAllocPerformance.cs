using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using DotNetBa.Internals.SpansAndSuch.Types;
using Xunit;
using Xunit.Abstractions;

namespace DotNetBa.Internals.SpansAndSuch.StackAllocation
{
    public class StackAllocPerformance
    {
        private readonly ITestOutputHelper _helper;

        public StackAllocPerformance(ITestOutputHelper helper)
        {
            _helper = helper;
        }

        [Fact]
        public void RefLocalBenchmarkTest()
        {
            var manualConfig = ManualConfig.Create(DefaultConfig.Instance)
                .With(loggers: new XunitTestOutputLogger(_helper));

            BenchmarkRunner.Run<StackAllocBenchmark>(manualConfig);
        }

        [MemoryDiagnoser]
        [InProcess]
        public class StackAllocBenchmark
        {
            [ParamsSource(nameof(Sizes))]
            public int Size { get; set; }

            public IEnumerable<int> Sizes => Enumerable.Range(1, 10).Select(_ => _ * 10000);


            [Benchmark(Baseline = true)]
            public void HeapAllocation()
            {
                var array = new SmallValueStruct[Size];
            }

            [Benchmark]
            public void UnsafeStackAllocation()
            {
                unsafe
                {
                    var array = stackalloc SmallValueStruct[Size];
                }
            }

            [Benchmark]
            public void SafeStackAllocation()
            {
                unsafe
                {
                    Span<SmallValueStruct> array = stackalloc SmallValueStruct[Size];
                }
            }
        }
    }
}