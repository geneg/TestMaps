using Code.MapColoniesWmtsFeature.Models;
using UnityEngine;

namespace Code
{
    public class WmtsRequestBuilder
    {
        private const string OutputSchemaURI = "http://schema.mapcolonies.com/raster";

        public WmtsRequestBuilder()
        {
            Debug.Log("RequestBuilder Created");
        }

        public GetRecordsRequest BuildQueryCatalogRequest(int maxRecords = 100)
        {
            return new GetRecordsRequest
            {
                Query = new Query(),
                OutputSchema = OutputSchemaURI,
                MaxRecords = maxRecords
            };
        }
    }
}