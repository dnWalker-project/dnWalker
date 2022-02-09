using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace dnWalker.Parameters.Serialization.Xml
{
    public class XmlParameterSetInfo : IParameterSetInfo
    {
        private readonly XElement _xml;

        public XElement Xml
        {
            get { return _xml; }
        }

        public XmlParameterSetInfo(XElement xml)
        {
            _xml = xml ?? throw new ArgumentNullException(nameof(xml));
        }

        public static XmlParameterSetInfo FromSet(IReadOnlyParameterSet set)
        {
            XmlSerializer serializer = new XmlSerializer();
            return new XmlParameterSetInfo(serializer.ToXml(set));
        }

        public IReadOnlyParameterSet Construct(IParameterContext context)
        {
            XmlDeserializer deserializer = new XmlDeserializer(context);
            return deserializer.ToParameterSet(_xml);
        }
    }
}
