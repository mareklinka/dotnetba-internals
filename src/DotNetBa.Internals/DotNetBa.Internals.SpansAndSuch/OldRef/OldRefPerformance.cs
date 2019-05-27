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

        [Fact]
        public void SettingValue()
        {
            var manualConfig = ManualConfig.Create(DefaultConfig.Instance)
                .With(loggers: new XunitTestOutputLogger(_helper));

            BenchmarkRunner.Run<SetValueBenchmark>(manualConfig);
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
            public void SetStructValues()
            {
                for (var i = 0; i < _data.Length; i++)
                {
                    DoSomethingWithStruct(_data[i]);
                }
            }

            [Benchmark]
            public void SetStructValuesByRef()
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

        [MemoryDiagnoser]
        public class SetValueBenchmark
        {
            private ValueStruct[] _data;

            [GlobalSetup]
            public void Setup()
            {
                _data = new ValueStruct[1000];
            }

            [Benchmark(Baseline = true)]
            [MethodImpl(MethodImplOptions.NoOptimization)]
            public void ArrayAccess()
            {
                for (var i = 0; i < _data.Length; i++)
                {
                    _data[i].Value1 = 1;
                    _data[i].Value2 = 3;
                }
            }

            [Benchmark]
            [MethodImpl(MethodImplOptions.NoOptimization)]
            public void CopyOut()
            {
                for (var i = 0; i < _data.Length; i++)
                {
                    var s = _data[i];

                    s.Value1 = 1;
                    s.Value2 = 3;

                    _data[i] = s;
                }
            }

            [Benchmark]
            [MethodImpl(MethodImplOptions.NoOptimization)]
            public void SetStructValuesByRef()
            {
                for (var i = 0; i < _data.Length; i++)
                {
                    DoSomethingWithStruct(ref _data[i]);
                }
            }

            [MethodImpl(MethodImplOptions.NoInlining)]
            private void DoSomethingWithStruct(ref ValueStruct s)
            {
                s.Value1 = 1;
                s.Value2 = 3;
            }
        }
    }
}