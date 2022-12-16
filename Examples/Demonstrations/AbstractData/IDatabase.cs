namespace Examples.Demonstrations.AbstractData
{
    public interface IDatabase
    {
        DataRecord[] GetRecords(int id);
        int GetCheckSum(int id);
    }
}
