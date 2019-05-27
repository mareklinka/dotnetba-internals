using Xunit;

namespace DotNetBa.Internals.SpansAndSuch.Types
{
    public struct BadStruct : IStruct
    {
        public int Value1;
        public int Value2;
        public int Value3;
        public int Value4;

        public double Value5;
        public double Value6;
        public double Value7;
        public double Value8;

        public int Val => Value1;
    }

    public interface IStruct
    {

    }

    public class StructTest
    {
        [Fact]
        public void Test()
        {
            var s1 = new BadStruct();
            var s2 = new BadStruct();

            IStruct iS = s1;
            IStruct iS2 = s2;

            Assert.Equal(s1, s2);
            Assert.True(iS == iS2);
        }

        private bool IsEqual(IStruct s1, IStruct s2)
        {
            return s1 == s2;
        }
    }
}