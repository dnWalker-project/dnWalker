using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.Demonstrations.Abstract
{
    public class DataRecord
    {
        public string Name;
        public string StrValue;
        public int IntValue;

        public string AsString() => StrValue;
        public int AsInt() => IntValue;
    }

    public interface IDatabase
    {
        DataRecord[] GetRecords(int id);
        int GetCheckSum(int id);
    }

    public abstract class DataObject
    {
        public int Id;
        public string Author;
        public int LastAccess;
        public int Created;

        public bool ReadData(IDatabase database)
        {
            DataRecord[] records = database.GetRecords(Id);
            // Debug.Assert(records != null);

            for (int i = 0; i < records.Length; ++i)
            {
                DataRecord dr = records[i];
                if (dr.Name == "LastAccess")
                {
                    if (LastAccess < dr.AsInt() &&
                        dr.AsInt() >= Created)
                    {
                        LastAccess = dr.AsInt();
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
                if (dr.Name == "Created")
                {
                    if (Created < dr.AsInt() &&
                        LastAccess >= dr.AsInt())
                    {
                        Created = dr.AsInt();
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
                if (dr.Name == "Author")
                {
                    Author = dr.AsString();
                    continue;
                }
                if (!SetValue(dr))
                {
                    return false;
                }
            }

            if (GetCheckSum() == database.GetCheckSum(Id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected int GetCheckSum()
        {
            return (Id +
                    Created *
                    LastAccess) % (2 << 31);
        }

        protected abstract bool SetValue(DataRecord record);
    }
}
