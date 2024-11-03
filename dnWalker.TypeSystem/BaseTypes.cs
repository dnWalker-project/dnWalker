using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TypeSystem
{
    internal class BaseTypes : IBaseTypes
    {
        private readonly ICorLibTypes _corLibTypes;

        public BaseTypes(IDefinitionProvider definitionProvider) //, ICorLibTypes corLibTypes)
        {
            _corLibTypes = definitionProvider.Context.MainModule.CorLibTypes;

            _lazyThread = new Lazy<TypeDefOrRefSig>(() =>
            {
                return definitionProvider.GetTypeDefinition("System.Threading.Thread").ToTypeSig().ToTypeDefOrRefSig();
            });

            _lazyDelegate = new Lazy<TypeDefOrRefSig>(() =>
            {
                return definitionProvider.GetTypeDefinition("System.Delegate").ToTypeSig().ToTypeDefOrRefSig();
            });
            
            _lazyException = new Lazy<TypeDefOrRefSig>(() =>
            {
                return definitionProvider.GetTypeDefinition("System.Exception").ToTypeSig().ToTypeDefOrRefSig();
            });
        }

        public TypeRef GetTypeRef(string @namespace, string name)
        {
            return _corLibTypes.GetTypeRef(@namespace, name);
        }

        public CorLibTypeSig Void
        {
            get
            {
                return _corLibTypes.Void;
            }
        }

        public CorLibTypeSig Boolean
        {
            get
            {
                return _corLibTypes.Boolean;
            }
        }

        public CorLibTypeSig Char
        {
            get
            {
                return _corLibTypes.Char;
            }
        }

        public CorLibTypeSig SByte
        {
            get
            {
                return _corLibTypes.SByte;
            }
        }

        public CorLibTypeSig Byte
        {
            get
            {
                return _corLibTypes.Byte;
            }
        }

        public CorLibTypeSig Int16
        {
            get
            {
                return _corLibTypes.Int16;
            }
        }

        public CorLibTypeSig UInt16
        {
            get
            {
                return _corLibTypes.UInt16;
            }
        }

        public CorLibTypeSig Int32
        {
            get
            {
                return _corLibTypes.Int32;
            }
        }

        public CorLibTypeSig UInt32
        {
            get
            {
                return _corLibTypes.UInt32;
            }
        }

        public CorLibTypeSig Int64
        {
            get
            {
                return _corLibTypes.Int64;
            }
        }

        public CorLibTypeSig UInt64
        {
            get
            {
                return _corLibTypes.UInt64;
            }
        }

        public CorLibTypeSig Single
        {
            get
            {
                return _corLibTypes.Single;
            }
        }

        public CorLibTypeSig Double
        {
            get
            {
                return _corLibTypes.Double;
            }
        }

        public CorLibTypeSig String
        {
            get
            {
                return _corLibTypes.String;
            }
        }

        public CorLibTypeSig TypedReference
        {
            get
            {
                return _corLibTypes.TypedReference;
            }
        }

        public CorLibTypeSig IntPtr
        {
            get
            {
                return _corLibTypes.IntPtr;
            }
        }

        public CorLibTypeSig UIntPtr
        {
            get
            {
                return _corLibTypes.UIntPtr;
            }
        }

        public CorLibTypeSig Object
        {
            get
            {
                return _corLibTypes.Object;
            }
        }

        private readonly Lazy<TypeDefOrRefSig> _lazyThread;

        public TypeDefOrRefSig Thread
        {
            get
            {
                return _lazyThread.Value;
            }
        }

        private readonly Lazy<TypeDefOrRefSig> _lazyDelegate;

        public TypeSig Delegate
        {
            get
            {
                return _lazyDelegate.Value;
            }
        }

        private readonly Lazy<TypeDefOrRefSig> _lazyException;

        public TypeDefOrRefSig Exception
        {
            get
            {
                return _lazyException.Value;
            }
        }

        public AssemblyRef AssemblyRef
        {
            get
            {
                return _corLibTypes.AssemblyRef;
            }
        }
    }
}