using System.Collections;

namespace dnWalker.TestWriter.TestModels
{
    internal class PackageReferenceCollection<T> : ICollection<PackageReference>
    {
        private readonly SortedDictionary<string, PackageReference> _packages = new SortedDictionary<string, PackageReference>(StringComparer.OrdinalIgnoreCase);

        public void Add(PackageReference item)
        {
            string name = item.Name ?? throw new InvalidOperationException("The package reference is not initialized, missing name");
            PackageVersion version = item.Version ?? throw new InvalidOperationException("The package reference is not initialized, missing version");

            if (_packages.TryGetValue(item.Name, out PackageReference? cur)) 
            {
                if (version.Version == null)
                {
                    // the adding reference version is "any" => just merge assets
                }
                else if (cur.Version!.Version == null)
                {
                    // the current version is "any"
                    cur.Version = item.Version;
                }
                else if (cur.Version.Version < item.Version.Version)
                {
                    cur.Version = item.Version;
                }

                MergeAssets(item, cur);
            }
            else
            {
                _packages.Add(name, item);
            }
        }

        private void MergeAssets(PackageReference newAssets, PackageReference targetAssets)
        {
            // do nothing and hope each one is including the same assets...
            // TODO: somehow decide how to handle this...
        }

        public void Clear()
        {
            _packages.Clear();
        }

        public bool Contains(PackageReference item)
        {
            string name = item.Name ?? throw new InvalidOperationException("The package reference is not initialized, missing name");

            return _packages.ContainsKey(name);
        }

        public void CopyTo(PackageReference[] array, int arrayIndex)
        {
            foreach (PackageReference pr in this)
            {
                array[arrayIndex++] = pr;
            }
        }

        public bool Remove(PackageReference item)
        {
            string name = item.Name ?? throw new InvalidOperationException("The package reference is not initialized, missing name");

            return _packages.Remove(name);
        }

        public int Count
        {
            get
            {
                return _packages.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public IEnumerator<PackageReference> GetEnumerator()
        {
            return _packages.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _packages.Values.GetEnumerator();
        }
    }
}