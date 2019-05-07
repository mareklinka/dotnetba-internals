using System;
using DotNetBa.Internals.SpansAndSuch.Types;
using Xunit;

namespace DotNetBa.Internals.SpansAndSuch.Readonly
{
    public class ReadonlyStuff
    {
        /*
         * Notes
         *
         * In parameters are read-only and passed by reference
         * Problem: non-readonly structs will require defensive copies in a lot of situations (property/method access)
         * Recommendation: it's best to only pass readonly structs as in parameters
         */

        private RefStruct MyRefStruct => new RefStruct();

        private ReadOnlyRefStruct MyRORefStruct => new ReadOnlyRefStruct();

        private ReadOnlyValueStruct _roValueStruct = new ReadOnlyValueStruct();

        private ValueStruct _roValueStruct2 = new ValueStruct {Value1 = 1};

        public void Regenerate()
        {
            _roValueStruct2 = new ValueStruct();
        }

        [Fact]
        public void InParameter()
        {
            var refStruct = new RefStruct();
            var roRefStruct = new ReadOnlyRefStruct();

            Function(refStruct);
            Function(roRefStruct);

            Function(MyRefStruct); // why is this weird?
            Function(MyRefStruct);
            Function(MyRefStruct);
            Function(MyRefStruct);

            Function(MyRORefStruct);
            Function(MyRORefStruct);
            Function(MyRORefStruct);
            Function(MyRORefStruct);

            Function(in _roValueStruct);
            Function(_roValueStruct);
            Function(_roValueStruct);
            Function(_roValueStruct);

            Function(in _roValueStruct2);
            Function(_roValueStruct2);
            Function(_roValueStruct2);
            Function(_roValueStruct2);
        }

        private void Function(in RefStruct s)
        {
        }

        private void Function(in MyObject o)
        {
            //o = new MyObject();
        }

        private void Function(in ReadOnlyRefStruct s)
        {
        }

        private void Function(in ReadOnlyValueStruct s)
        {
            // struct is readonly - no defensive copy
            Console.WriteLine(s.Val);
        }

        private void Function(in ValueStruct s)
        {
            // struct is not readonly - accessing a property/method requires a defensive copy
            Console.WriteLine(s.Val);
        }
    }
}
