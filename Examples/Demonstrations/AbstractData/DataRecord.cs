namespace Examples.Demonstrations.AbstractData
{
    public class DataRecord
    {
        public string Name;
        public string StrValue;
        public int IntValue;

        public string AsString() => StrValue;
        public int AsInt() => IntValue;
    }
}
