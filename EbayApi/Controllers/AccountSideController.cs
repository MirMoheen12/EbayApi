using EbayApi.AbaysideModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace EbayApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountSideController : ControllerBase
    {
        [HttpGet]
        public string GetSessionId()
        {
            string devId = "cefccd16-532f-442e-bbc5-4fd962fecf94";
            string appId = "RomanAzi-Inventor-SBX-4d7513f64-220e8727";
            string certId = "SBX-d7513f64924e-7ad9-4a67-bddd-1437";
            string endPoint = "https://api.sandbox.ebay.com/ws/api.dll";
            string ruName = "PARTS_PROPER_LL-RomanAzi-Invent-galpqrqcm";
            string username = "your-ebay-username";
            string password = "your-ebay-password";

            string requestUrl = endPoint + "?callname=GetSessionID&appid=" + appId + "&siteid=0&version=967&Routing=default";
            string requestBody = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                 "<GetSessionIDRequest xmlns=\"urn:ebay:apis:eBLBaseComponents\">" +
                                 "<RuName>" + ruName + "</RuName>" +
                                 "</GetSessionIDRequest>";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
            request.Method = "POST";
            request.Headers.Add("X-EBAY-API-COMPATIBILITY-LEVEL", "967");
            request.Headers.Add("X-EBAY-API-DEV-NAME", devId);
            request.Headers.Add("X-EBAY-API-APP-NAME", appId);
            request.Headers.Add("X-EBAY-API-CERT-NAME", certId);
            request.ContentType = "text/xml";
            request.Headers.Add("X-EBAY-API-REQUEST-ENCODING", "UTF-8");

            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(requestBody);
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string responseXml = reader.ReadToEnd();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(responseXml);
                string sessionId = xmlDoc.SelectSingleNode("//SessionID").InnerText;

                // Use the sessionId to authenticate further API requests
                return sessionId;
            }
        }

        public static string Serialize<T>(T dataToSerialize)
        {
            try
            {
                var stringwriter = new System.IO.StringWriter();
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(stringwriter, dataToSerialize);
                return stringwriter.ToString();
            }
            catch
            {
                throw;
            }
        }
    }
}
