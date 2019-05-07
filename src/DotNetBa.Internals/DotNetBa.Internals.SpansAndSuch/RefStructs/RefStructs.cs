using CommandLine;
using DotNetBa.Internals.SpansAndSuch.Types;
using Xunit;

namespace DotNetBa.Internals.SpansAndSuch.RefStructs
{
    public class RefStructs
    {
        /*
         * Notes
         *
         * Primary motivation: spans
         * Same limitations, but custom made
         * Why it might be helpful: ref structs can be fields of other ref structs
         */
        [Fact]
        public RefStruct RefStruct()
        {
            var s = new StructTuple();

            return s.s1;
        }

        public ref struct StructTuple
        {
            public RefStruct s1;
            public RefStruct s2;
        }
    }
}
