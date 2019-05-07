using System;
using System.Runtime.InteropServices;
using DotNetBa.Internals.SpansAndSuch.Types;
using Xunit;

namespace DotNetBa.Internals.SpansAndSuch.Spans
{
    /*
     * Notes
     *
     * Encapsulates any kind of memory
     * Allows pointer coercion (reinterpret cast)
     * Is ref struct and cannot leave stack under any conditions (not in async methods, not in a class field, not in captured scope)
     * Array-like performance
     * Allocation-free operations
     * Makes use of ref returns and ref readonly returns
     */
    public class Spans
    {
        [Fact]
        public void SpanFromHeap()
        {
            var structs = new Span<ValueStruct>(new ValueStruct[1000]);
        }

        [Fact]
        public void SpanFromGlobalMemory()
        {
            var valueStructArrayPointer = Marshal.AllocHGlobal(Marshal.SizeOf<ValueStruct>() * 1000);

            try
            {
                unsafe
                {
                    var structs = new Span<ValueStruct>((ValueStruct*)valueStructArrayPointer,
                        Marshal.SizeOf<ValueStruct>() * 1000);
                }
            }
            finally
            {
                Marshal.FreeHGlobal(valueStructArrayPointer);
            }
        }

        [Fact]
        public void SpanFromStack()
        {
            Span<ValueStruct> structs = stackalloc ValueStruct[1000];
        }

        [Fact]
        public void Coercion()
        {
            var byteArray = new byte[] {100, 200, 100, 200, 200, 100, 200, 100};

            var byteSpan = byteArray.AsSpan();
            Assert.Equal(byteArray.Length, byteSpan.Length);

            var intSpan = MemoryMarshal.Cast<byte, int>(byteSpan);
            Assert.Equal(byteArray.Length / 4, intSpan.Length);

            var doubleSpan = MemoryMarshal.Cast<int, double>(intSpan);
            Assert.Equal(byteArray.Length / 8, doubleSpan.Length);
        }

        [Fact]
        public Span<byte> Limitations()
        {
            Span<byte> array = stackalloc byte[100];

            //object o = array;

            //return array;
            return Span<byte>.Empty;
        }
    }
}
