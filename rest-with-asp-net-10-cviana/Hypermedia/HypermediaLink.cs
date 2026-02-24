using System.Xml.Serialization;
using rest_with_asp_net_10_cviana.Hypermedia.Constants;

namespace rest_with_asp_net_10_cviana.Hypermedia
{
    public class HypermediaLink
    {
        [XmlAttribute]
        public string Rel {get; set;} = string.Empty;

        [XmlAttribute]
        public string Href {get; set;} = string.Empty;

        [XmlAttribute]
        public string Type {get; set;} = ResponseTypeFormat.JSON;
        
        [XmlAttribute]
        public string Action {get; set;} = string.Empty;
    }
}