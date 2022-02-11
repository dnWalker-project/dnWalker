using dnWalker.Parameters;
using dnWalker.TypeSystem;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.TestClasses
{
    /// <summary>
    /// Base class for all assertion schemas.
    /// </summary>
    public abstract class AssertionSchema
    {
    }

    public class ExceptionSchema : AssertionSchema
    { 
        public static readonly ExceptionSchema NoException = new ExceptionSchema(TypeSignature.Empty);

        public ExceptionSchema(TypeSignature exceptionType)
        {
            ExceptionType = exceptionType;
        }

        public TypeSignature ExceptionType { get; }
    }

    public class ReturnValueSchema : AssertionSchema
    {
        public ReturnValueSchema(IParameter returnValueParameter)
        {
            ReturnValue = returnValueParameter ?? throw new ArgumentNullException(nameof(returnValueParameter));
        }

        public IParameter ReturnValue
        {
            get;
        }
    }

    public class ArrayElementSchema : AssertionSchema
    {
        private readonly ParameterRef _parameterRef;
        private readonly IReadOnlyParameterSet _baseSet;
        private readonly IReadOnlyParameterSet _execSet;
        private readonly int[] _positions;

        public ArrayElementSchema(ParameterRef parameterRef, IReadOnlyParameterSet baseSet, IReadOnlyParameterSet execSet, int[] positions)
        {
            _parameterRef = parameterRef;
            _baseSet = baseSet ?? throw new ArgumentNullException(nameof(baseSet));
            _execSet = execSet ?? throw new ArgumentNullException(nameof(execSet));
            _positions = positions ?? throw new ArgumentNullException(nameof(positions));
        }

        public IArrayParameter InputState
        {
            get { return _parameterRef.Resolve<IArrayParameter>(_baseSet) ?? throw new Exception("Could not resolve the parameter."); }
        }
        public IArrayParameter OutputState
        {
            get { return _parameterRef.Resolve<IArrayParameter>(_execSet) ?? throw new Exception("Could not resolve the parameter."); }
        }
        public int[] Positions
        {
            get { return _positions; }
        }
    }

    public class ObjectFieldSchema : AssertionSchema
    {
        private readonly ParameterRef _parameterRef;
        private readonly IReadOnlyParameterSet _baseSet;
        private readonly IReadOnlyParameterSet _execSet;
        private readonly string[] _fields;

        public ObjectFieldSchema(ParameterRef parameterRef, IReadOnlyParameterSet baseSet, IReadOnlyParameterSet execSet, string[] fields)
        {
            _parameterRef = parameterRef;
            _baseSet = baseSet ?? throw new ArgumentNullException(nameof(baseSet));
            _execSet = execSet ?? throw new ArgumentNullException(nameof(execSet));
            _fields = fields ?? throw new ArgumentNullException(nameof(fields));
        }

        public IObjectParameter InputState
        {
            get { return _parameterRef.Resolve<IObjectParameter>(_baseSet) ?? throw new Exception("Could not resolve the parameter."); }
        }
        public IObjectParameter OutputState
        {
            get { return _parameterRef.Resolve<IObjectParameter>(_execSet) ?? throw new Exception("Could not resolve the parameter."); }
        }
        public string[] Fields
        {
            get { return _fields; }
        }
    }

    [Obsolete("Maybe a bad idea? Just have a single schema per test - easier to detect what went wrong...")]
    public class MergedSchema : AssertionSchema
    {
        // we can merge some assertions together, probably
        // but maybe later, better design is to have only one assert per test
    }
}
