using System;
using DotNetBa.Internals.SpansAndSuch.Types;
using Xunit;

namespace DotNetBa.Internals.SpansAndSuch.StackAllocation
{
    /*
     * Notes
     *
     * Allocates memory on the stack
     * Can be used in both safe and unsafe contexts (long live Span<T>)
     * Alleviates GC pressure (no heap alloc)
     * Better memory locality
     * Minimized by maximum stack size (~1MB)
     * Consumed memory is collected DETERMINISTICALLY upon stack unwind
     */
    public class StackAllocation
    {
        [Fact]
        public void StackAllocWithPointers()
        {
            unsafe
            {
                var structs = stackalloc ValueStruct[1000];
            }
        }

        [Fact]
        public void StackAllocWithSpan()
        {
            Span<ValueStruct> structs = stackalloc ValueStruct[1000];
        }
    }
}
