namespace dnWalker.TestGenerator.Tests.Templates
{
    public class ConcreteClass
    {
        private int _field1;
        private int _field2;

        public virtual int Method() { return 0; }

        private ConcreteClass[] _array;
        private ConcreteClass _peer;

        public virtual ConcreteClass GetPeer() { return _peer; }
    }
}
