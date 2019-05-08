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
         *
         * Ref readonly return enforces immutability on the struct but also requires defensive copying for non-readonly structs
         */

        private RefStruct MyRefStruct => new RefStruct();

        private ReadOnlyRefStruct MyRORefStruct => new ReadOnlyRefStruct();

        private readonly ReadOnlyValueStruct _roValueStruct = new ReadOnlyValueStruct();

        private readonly ValueStruct _roValueStruct2 = new ValueStruct {Value1 = 1};

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

        [Fact]
        public int ReadonlyReturn()
        {
            var a = new ValueStruct[100];
            var b = new ReadOnlyValueStruct[100];

            ref readonly var value = ref ReturnReadonly(a);
            ref readonly var value2 = ref ReturnReadonly(b);
            //x = new ValueStruct();

            var x = 1 + value.Val;
            var y = 1 + value2.Val;

            return x + y;
        }

        [Fact]
        public void TernaryRef()
        {
            var a = new ReadOnlyValueStruct[100];

            ref var x = ref (a.Length >= 2 ? ref ReturnN(a, 1) : ref ReturnN(a, 0));
            ref readonly var y = ref (a.Length >= 2 ? ref ReturnReadonlyN(a, 1) : ref ReturnReadonlyN(a, 0));
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

        private ref readonly ValueStruct ReturnReadonly(ValueStruct[] array)
        {
            return ref array[0];
        }

        private ref readonly ReadOnlyValueStruct ReturnReadonly(ReadOnlyValueStruct[] array)
        {
            return ref array[0];
        }

        private ref ReadOnlyValueStruct ReturnN(ReadOnlyValueStruct[] array, int i)
        {
            return ref array[i];
        }

        private ref readonly ReadOnlyValueStruct ReturnReadonlyN(ReadOnlyValueStruct[] array, int i)
        {
            return ref array[i];
        }
    }
}
