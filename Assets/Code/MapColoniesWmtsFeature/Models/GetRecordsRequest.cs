using System.Text;
using System.Xml.Serialization;

namespace Code.MapColoniesWmtsFeature.Models
{
    [XmlRoot(ElementName = "GetRecords", Namespace = "http://www.opengis.net/cat/csw/2.0.2")]
    public class GetRecordsRequest
    {
        [XmlAttribute("outputFormat")] public string OutputFormat { get; set;} = "application/xml";
        [XmlAttribute("outputSchema")] public string OutputSchema { get; set; }
        [XmlAttribute("resultType")] public string ResultType { get; set; } = "results";
        [XmlAttribute("service")] public string Service { get; set; } = "CSW";
        [XmlAttribute("version")] public string Version { get; set; } = "2.0.2";
        [XmlAttribute("startPosition")] public int StartPosition { get; set; } = 1;
        [XmlAttribute("maxRecords")] public int MaxRecords { get; set; }
        [XmlElement("Query")] public Query Query { get; set; }
        
        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces Xmlns { get; set; } = new XmlSerializerNamespaces(new[]
        {
            new System.Xml.XmlQualifiedName("mc", "http://schema.mapcolonies.com/rester"),
            new System.Xml.XmlQualifiedName("csw", "http://www.opengis.net/cat/csw/2.0.2"),
            new System.Xml.XmlQualifiedName("ogc", "http://www.opengis.net/ogc")
        });
    }

    public class Query
    {
        [XmlAttribute("typeNames")] public string TypeNames { get; set; } = "csw:Record";
        [XmlElement("ElementSetName")] public string ElementSetName { get; set; } = "full";
    }
}