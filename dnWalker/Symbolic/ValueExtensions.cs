using dnWalker.Symbolic;

using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic
{
    public static class ValueExtensions
    {
        private const string LocationMappingAttribute = "location-mapping";

        public static IDictionary<Location, uint> GetLocationMapping(this ExplicitActiveState cur)
        {
            if (!cur.PathStore.CurrentPath.TryGetPathAttribute(LocationMappingAttribute, out IDictionary<Location, uint> mapping))
            {
                mapping = new Dictionary<Location, uint>();
                cur.SetLocationMapping(mapping);
            }
            return mapping;
        }

        public static void SetLocationMapping(this ExplicitActiveState cur, IDictionary<Location, uint> mapping)
        {
            cur.PathStore.CurrentPath.SetPathAttribute(LocationMappingAttribute, mapping);
        }
    }
}
