using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Networking;

namespace Code.Utils
{
    public static class XmlHelper
    {
        public static string SerializeToXml<T>(T obj)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (MemoryStream memoryStream = new MemoryStream())
            {
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Encoding = new UTF8Encoding(false),
                    Indent = true,
                    OmitXmlDeclaration = false
                };

                using (XmlWriter writer = XmlWriter.Create(memoryStream, settings))
                {
                    serializer.Serialize(writer, obj);
                }

                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }
        
        public static T DeserializeFromXml<T>(string xmlData)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StringReader reader = new StringReader(xmlData))
            {
                return (T)serializer.Deserialize(reader);
            }
        }
        
        public static async Task<T> SendXmlAsync<T>(string url, string xmlData = "") where T : class
        {
            using (UnityWebRequest request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST))
            {
                request.SetRequestHeader("Content-Type", "application/xml");

                if (xmlData != string.Empty)
                {
                    byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(xmlData);
                    request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                }
                
                request.downloadHandler = new DownloadHandlerBuffer();

                UnityWebRequestAsyncOperation asyncOp = request.SendWebRequest();

                while (!asyncOp.isDone)
                {
                    await Task.Yield(); 
                }

                if (request.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log("Request succeeded. Response: " + request.downloadHandler.text);

                    string responseXml = request.downloadHandler.text;
                    T response = DeserializeFromXml<T>(responseXml);

                    return response;
                }
                
                Debug.LogError("Request failed. Error: " + request.error);
                return null;
            }
        }
    }
}