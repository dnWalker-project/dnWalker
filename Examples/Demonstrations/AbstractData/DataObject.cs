using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.Demonstrations.AbstractData
{

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
                if (dr.Name == "Author" && !string.IsNullOrEmpty(dr.AsString()))
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
            return (Id * 1671941 + Created * 8329 + LastAccess * 4909 + 54629) % (1 << 30);
        }

        public abstract bool SetValue(DataRecord record);
    }
}
