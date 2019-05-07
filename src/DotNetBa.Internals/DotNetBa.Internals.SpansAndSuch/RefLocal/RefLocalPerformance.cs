using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using DotNetBa.Internals.SpansAndSuch.Types;
using Xunit;
using Xunit.Abstractions;

namespace DotNetBa.Internals.SpansAndSuch.RefLocal
{
    public class RefLocalPerformance
    {
        private readonly ITestOutputHelper _helper;

        public RefLocalPerformance(ITestOutputHelper helper)
        {
            _helper = helper;
        }

        [Fact]
        public void RefLocalBenchmarkTest()
        {
            var manualConfig = ManualConfig.Create(DefaultConfig.Instance)
                .With(loggers: new XunitTestOutputLogger(_helper));

            BenchmarkRunner.Run<RefLocalBenchmark>(manualConfig);
        }

        [MemoryDiagnoser]
        public class RefLocalBenchmark
        {
            private ValueStruct[] _data;

            [GlobalSetup]
            public void Setup()
            {
                _data = new ValueStruct[10000];

                for (var i = 0; i < _data.Length; i++)
                {
                    _data[i].Value1 = i;
                }
            }

            [Benchmark(Baseline = true)]
            public ValueStruct GetMaxByValue()
            {
                var maxRef = _data[0];

                for (var i = 1; i < _data.Length; i++)
                {
                    if (maxRef.Value1 < _data[i].Value1)
                    {
                        maxRef = _data[i];
                    }
                }

                return maxRef;
            }

            [Benchmark]
            public ref ValueStruct GetMaxByRef()
            {
                ref var maxRef = ref _data[0];

                for (var i = 1; i < _data.Length; i++)
                {
                    if (maxRef.Value1 < _data[i].Value1)
                    {
                        maxRef = ref _data[i];
                    }
                }

                return ref maxRef;
            }
        }
    }
}