using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TypeSystem
{
    public readonly partial struct TypeSignature
    {
        public bool IsString
        {
            get
            {
                return _type != null && new SigComparer().Equals(_type, _type.Module.CorLibTypes.String);
            }
        }
        public bool IsChar
        {
            get
            {
                return _type != null && new SigComparer().Equals(_type, _type.Module.CorLibTypes.Char);
            }
        }
        public bool IsBoolean
        {
            get
            {
                return _type != null && new SigComparer().Equals(_type, _type.Module.CorLibTypes.Boolean);
            }
        }
        public bool IsByte
        {
            get
            {
                return _type != null && new SigComparer().Equals(_type, _type.Module.CorLibTypes.Byte);
            }
        }
        public bool IsSByte
        {
            get
            {
                return _type != null && new SigComparer().Equals(_type, _type.Module.CorLibTypes.SByte);
            }
        }
        public bool IsUInt16
        {
            get
            {
                return _type != null && new SigComparer().Equals(_type, _type.Module.CorLibTypes.UInt16);
            }
        }
        public bool IsInt16
        {
            get
            {
                return _type != null && new SigComparer().Equals(_type, _type.Module.CorLibTypes.Int16);
            }
        }
        public bool IsUInt32
        {
            get
            {
                return _type != null && new SigComparer().Equals(_type, _type.Module.CorLibTypes.UInt32);
            }
        }
        public bool IsInt32
        {
            get
            {
                return _type != null && new SigComparer().Equals(_type, _type.Module.CorLibTypes.Int32);
            }
        }
        public bool IsUInt64
        {
            get
            {
                return _type != null && new SigComparer().Equals(_type, _type.Module.CorLibTypes.UInt64);
            }
        }
        public bool IsInt64
        {
            get
            {
                return _type != null && new SigComparer().Equals(_type, _type.Module.CorLibTypes.Int64);
            }
        }
        public bool IsDouble
        {
            get
            {
                return _type != null && new SigComparer().Equals(_type, _type.Module.CorLibTypes.Double);
            }
        }
        public bool IsSingle
        {
            get
            {
                return _type != null && new SigComparer().Equals(_type, _type.Module.CorLibTypes.Single);
            }
        }
        public bool IsVoid
        {
            get
            {
                return _type != null && new SigComparer().Equals(_type, _type.Module.CorLibTypes.Void);
            }
        }
    }
}