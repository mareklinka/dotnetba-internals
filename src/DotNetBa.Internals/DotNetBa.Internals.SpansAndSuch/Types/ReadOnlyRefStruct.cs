namespace DotNetBa.Internals.SpansAndSuch.Types
{
    public readonly ref struct ReadOnlyRefStruct
    {
        public ReadOnlyRefStruct(int value1, int value2, int value3, int value4, double value5, double value6, double value7, double value8)
        {
            Value1 = value1;
            Value2 = value2;
            Value3 = value3;
            Value4 = value4;

            Value5 = value5;
            Value6 = value6;
            Value7 = value7;
            Value8 = value8;
        }

        public readonly int Value1;
        public readonly int Value2;
        public readonly int Value3;
        public readonly int Value4;

        public readonly double Value5;
        public readonly double Value6;
        public readonly double Value7;
        public readonly double Value8;
    }
}