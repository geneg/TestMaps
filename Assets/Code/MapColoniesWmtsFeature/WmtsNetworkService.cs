using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Code.MapColoniesWmtsFeature.Models;
using Code.Utils;
using UnityEngine;
using Zenject;

namespace Code
{
    public class WmtsNetworkService : IInitializable, IDisposable
    {
        private const string catalogUrl = "https://catalog.mapcolonies.net/api/raster/v1/csw?service=CSW";
        private const string Scheme = "WMTS";
        private const string Token = "eyJhbGciOiJSUzI1NiIsImtpZCI6Im1hcC1jb2xvbmllcy1pbnQifQ.eyJhbyI6WyJodHRwczovL2FwcC1pbnQtY2xpZW50LXJvdXRlLWludGVncmF0aW9uLmFwcHMuajFsazNuanAuZWFzdHVzLmFyb2FwcC5pbyIsImh0dHBzOi8vYXBwLWludC1jbGllbnQtdG9vbHMtcm91dGUtaW50ZWdyYXRpb24uYXBwcy5qMWxrM25qcC5lYXN0dXMuYXJvYXBwLmlvIiwiaHR0cDovL2xvY2FsaG9zdDozMDAwIl0sImQiOlsicmFzdGVyIiwicmFzdGVyV21zIiwicmFzdGVyRXhwb3J0IiwiZGVtIiwidmVjdG9yIiwiM2QiXSwiaWF0IjoxNjc0NjMyMzQ2LCJzdWIiOiJtYXBjb2xvbmllcy1hcHAiLCJpc3MiOiJtYXBjb2xvbmllcy10b2tlbi1jbGkifQ.e-4SmHNOE8FwpcJoHdp-3Dh6D8GqCwM5wZfZIPrivGhfeKdihcsjEj_WN2jWN-ULha_ytZN5gRusLjwikNwgbF6hvb-QTDe3bEHPAjtgpZmF4HaJze8e6VPDF1tTC52CHDzNnwkUGAH1tnVGq10SnyhsGDezUChTVeBeVu-swTI58qCjemUQRw7-Q03uSEH24AkbX2CC1_rNwulo7ChglyTdn01tTWPsPjIuDjeixxm2CUmUHpfZzroaSzwof7ByQe22o3tFddje6ItNLBUC_VN7UfNLa_QPSVbIuNac-iMGFbK-RIyXUK8mp1AwddvSGsBUYcDs8fWMLzKhItljnw";
       
        private readonly WmtsRequestBuilder _wmtsRequestBuilder;
        private string _capabilitiesUrl = string.Empty;
        
        public WmtsNetworkService(WmtsRequestBuilder wmtsRequestBuilder)
        {
            _wmtsRequestBuilder = wmtsRequestBuilder;
        }

        public async void Initialize()
        {
            GetRecordsResponse catalogRecords = await QueryCatalog(TokenizeUrl(catalogUrl));
            Dictionary<string, MCRasterRecord> records = ExtractRecordsByScheme(catalogRecords, Scheme);
            
            _capabilitiesUrl = GetCapabilitiesUrl(records);

            GetCapabilitiesResponse capabilities = await QueryCapabilities(TokenizeUrl(_capabilitiesUrl));
            Dictionary<string, Layer> layers = ExtractLayers(capabilities, records);
            
            
            
           // HashSet<string> linkNames = new HashSet<string>(links.Select(link => link.name));
           // List<Layer> matchingLayers = capabilities.Contents.Layers
            //    .Where(layer => linkNames.Contains(layer.Identifier))
            //    .ToList();
            
            Debug.Log("cap");
        }

        private async Task<GetCapabilitiesResponse> QueryCapabilities(string tokenizedCapabilitiesUrl)
        {
            return await XmlHelper.SendXmlAsync<GetCapabilitiesResponse>(tokenizedCapabilitiesUrl);
        }

        private Dictionary<string, MCRasterRecord> ExtractRecordsByScheme(GetRecordsResponse catalogRecords, string scheme)
        {
            GetRecordsResponseSearchResults rawRecords = catalogRecords.SearchResults.First();
            Dictionary<string, MCRasterRecord> records = new Dictionary<string, MCRasterRecord>();
            
            foreach (MCRasterRecord rasterRecord in rawRecords.MCRasterRecord)
            {
                records.Add(rasterRecord.links.First(x => x.scheme == scheme).name, rasterRecord);
            }

            return records;
        }
        
        private Dictionary<string, Layer> ExtractLayers(GetCapabilitiesResponse capabilities, Dictionary<string, MCRasterRecord> records)
        {
            List<Layer> rawLayers = capabilities.Contents.Layers;
            Dictionary<string, Layer> layers = new Dictionary<string, Layer>();
            
            foreach (Layer layer in rawLayers)
            {
                if (!records.Keys.Contains(layer.Identifier))
                {
                    Debug.Log(layer.Identifier + " capabilities layer not found in catalog records");
                    continue;
                }
                
                layers.Add(layer.Identifier, layer);
            }

            return layers;
        }
        private string TokenizeUrl(string url)
        {
            string tokenParam = "token=" + Token;
    
            if (url.Contains("?"))
            {
                if (url.Contains("token="))
                {
                    UriBuilder uriBuilder = new UriBuilder(url);
                    NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
                    query["token"] = Token;
                    uriBuilder.Query = query.ToString();
                    return uriBuilder.ToString();
                }
                
                return url + "&" + tokenParam;
            }
            else
            {
                // Add a query string with the token
                return url + "?" + tokenParam;
            }
        }
        
        public async Task<GetRecordsResponse> QueryCatalog(string url)
        {
            GetRecordsRequest getRecordsRequest = _wmtsRequestBuilder.BuildQueryCatalogRequest();
            string recordsRequestXml = XmlHelper.SerializeToXml(getRecordsRequest);
            
            return await XmlHelper.SendXmlAsync<GetRecordsResponse>(url, recordsRequestXml);
        }

        private string GetCapabilitiesUrl(Dictionary<string, MCRasterRecord> records)
        {
            return records.First().Value.links.First(x => x.scheme == Scheme).Value;
        }
        public void Dispose()
        {
            Debug.Log("MapColoniesService disposed");
        }
        
    }
}