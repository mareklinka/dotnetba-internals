using System;
using Xunit;

namespace DotNetBa.Internals.SpansAndSuch.Memory
{
    public class Memory
    {
        /*
         * Notes
         *
         * Not ref-struct
         * Can be passed to async methods, be a field etc.
         * .Span gets a standard span that can be used in the current scope
         * Can encapsulate heap memory only (GC will handle memory moves and such)
         */
        [Fact]
        public void FromArray()
        {
            var array = new int[100];

            var span = array.AsMemory().Span;
        }

        [Fact]
        public void FromStack()
        {
            // Memory<int> array = stackalloc int[100];
        }

        [Fact]
        public void Boxing()
        {
            var array = new int[100];

            object o = array.AsMemory();
        }

        private class MemoryHolder
        {
            private Memory<int> _memory;
            // private Span<int> _span;

            public MemoryHolder(Memory<int> m)
            {
                _memory = m;
            }
        }
    }
}
