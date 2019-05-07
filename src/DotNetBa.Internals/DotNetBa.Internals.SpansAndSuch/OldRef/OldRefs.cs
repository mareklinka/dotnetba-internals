using DotNetBa.Internals.SpansAndSuch.Types;
using Xunit;

namespace DotNetBa.Internals.SpansAndSuch.OldRef
{
    /*
     * Notes
     *
     * PassByValue vs PassByReference
     * Performance significantly better for ref-passing of large structs
     * Problem: you need a method with ref parameters, no way to store references - we must always dereference
     */
    public class OldRefs
    {
        [Fact]
        public void PassObjectToFunction()
        {
            var o = new MyObject();
            ObjectFunction(o);
        }

        [Fact]
        public void PassStructToFunction()
        {
            var s = new ValueStruct();
            ValueFunction(s);
        }

        [Fact]
        public void PassStructToFunctionByRef()
        {
            var s = new ValueStruct();
            ValueFunction(ref s);
        }

        private void ObjectFunction(MyObject o)
        {
        }

        private void ValueFunction(ValueStruct s)
        {
        }

        private void ValueFunction(ref ValueStruct s)
        {
        }
    }
}
