using System;
using DotNetBa.Internals.SpansAndSuch.Types;
using Xunit;

namespace DotNetBa.Internals.SpansAndSuch.RefLocal
{
    /*
     * Notes
     *
     * Finally able to store references locally, no need to dereference (and thus copy structs)
     * Functions might return references that are safe (not references to local values)
     * Allows for speeding up certain struct-heavy computation
     */
    public class RefLocals
    {
        [Fact]
        public void CreateRefLocal()
        {
            var x = 5;

            ref var xRef = ref x;

            xRef = 6;

            Assert.Equal(6, x);
        }

        [Fact]
        public void ReassignRefLocal()
        {
            var x = 5;
            var y = 10;

            ref var r = ref x;
            r = 6;

            r = ref y;
            r = 11;

            Assert.Equal(6, x);
            Assert.Equal(11, y);
        }

        [Fact]
        public void FindRefMax()
        {
            var array = new ValueStruct[1000];
            var r = new Random();

            for (var i = 0; i < array.Length; i++)
            {
                array[i].Value1 = r.Next(100000);
            }

            var maxValue = GetMax(array);
            ref var maxReference = ref GetMaxRef(array);
        }

        private ValueStruct GetMax(ValueStruct[] array)
        {
            var maxValue = array[0];

            for (var i = 1; i < array.Length; i++)
            {
                if (maxValue.Value1 < array[i].Value1)
                {
                    maxValue = array[i];
                }
            }

            return maxValue;
        }

        private ref ValueStruct GetMaxRef(ValueStruct[] array)
        {
            ref var maxRef = ref array[0];

            for (var i = 1; i < array.Length; i++)
            {
                if (maxRef.Value1 < array[i].Value1)
                {
                    maxRef = ref array[i];
                }
            }

            return ref maxRef;
        }
    }
}