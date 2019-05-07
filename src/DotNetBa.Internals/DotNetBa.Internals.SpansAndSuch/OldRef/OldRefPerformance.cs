using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using DotNetBa.Internals.SpansAndSuch.Types;
using Xunit;
using Xunit.Abstractions;

namespace DotNetBa.Internals.SpansAndSuch.OldRef
{
    public class OldRefPerformance
    {
        private readonly ITestOutputHelper _helper;

        public OldRefPerformance(ITestOutputHelper helper)
        {
            _helper = helper;
        }

        [Fact]
        public void SlowCode()
        {
            var manualConfig = ManualConfig.Create(DefaultConfig.Instance)
                .With(loggers: new XunitTestOutputLogger(_helper));

            BenchmarkRunner.Run<ArrayInitBenchmark>(manualConfig);
        }

        [MemoryDiagnoser]
        public class ArrayInitBenchmark
        {
            private ValueStruct[] _data;

            [GlobalSetup]
            public void Setup()
            {
                _data = new ValueStruct[1000];
            }

            [Benchmark(Baseline = true)]
            public void SetStructValue()
            {
                for (var i = 0; i < _data.Length; i++)
                {
                    DoSomethingWithStruct(_data[i]);
                }
            }

            [Benchmark]
            public void SetStructValueByRef()
            {
                for (var i = 0; i < _data.Length; i++)
                {
                    DoSomethingWithStruct(ref _data[i]);
                }
            }

            [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
            private void DoSomethingWithStruct(ref ValueStruct s)
            {
            }

            [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
            private void DoSomethingWithStruct(ValueStruct s)
            {
            }
        }
    }
}