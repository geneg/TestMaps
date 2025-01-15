using System.Collections.Generic;
using System.Xml.Serialization;

namespace Code.MapColoniesWmtsFeature.Models
{
    [XmlRoot(ElementName = "Capabilities", Namespace = "http://www.opengis.net/wmts/1.0")]
    public class GetCapabilitiesResponse
    {
        public Contents Contents { get; set; }
    }

    public class Contents
    {
        [XmlElement(ElementName = "Layer")]
        public List<Layer> Layers { get; set; } = new(); // Assuming multiple links can exist
    }


    public class Layer
    {
        [XmlElement(Namespace = "http://www.opengis.net/ows/1.1")]
        public string Title { get; set; }

        [XmlElement(Namespace = "http://www.opengis.net/ows/1.1")]
        public string Identifier { get; set; }

        public Style Style { get; set; }

        public TileMatrixSetLink TileMatrixSetLink { get; set; }

        [XmlElement(ElementName = "ResourceURL")]
        public ResourceURL ResourceURL { get; set; }
    }

    public class Style
    {
        [XmlElement(Namespace = "http://www.opengis.net/ows/1.1")]
        public string Identifier { get; set; }
    }

    public class TileMatrixSetLink
    {
        public string TileMatrixSet { get; set; }
    }

    public class ResourceURL
    {
        [XmlAttribute] public string format { get; set; }

        [XmlAttribute] public string resourceType { get; set; }

        [XmlAttribute] public string template { get; set; }
    }
}