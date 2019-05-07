using System;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using DotNetBa.Internals.SpansAndSuch.Types;
using Xunit;
using Xunit.Abstractions;

namespace DotNetBa.Internals.SpansAndSuch.Readonly
{
    public class ReadonlyStuffPerformance
    {
        private readonly ITestOutputHelper _helper;

        public ReadonlyStuffPerformance(ITestOutputHelper helper)
        {
            _helper = helper;
        }

        [Fact]
        public void DefensiveCopy()
        {
            var manualConfig = ManualConfig.Create(DefaultConfig.Instance)
                .With(loggers: new XunitTestOutputLogger(_helper));

            BenchmarkRunner.Run<ReadonlyBenchmark>(manualConfig);
        }

        public class ReadonlyBenchmark
        {
            private readonly ReadOnlyValueStruct _roData = new ReadOnlyValueStruct();
            private readonly ValueStruct _data = new ValueStruct();

            [Benchmark(Baseline = true)]
            public void NormalStruct()
            {
                Function(_data);
            }

            [Benchmark]
            public void ReadOnlyStruct()
            {
                Function(_roData);
            }

            public int Function(in ReadOnlyValueStruct s)
            {
                return s.Value1 + s.Val;
            }

            public int Function(in ValueStruct s)
            {
                return s.Value1 + s.Val;
            }
        }
    }
}
