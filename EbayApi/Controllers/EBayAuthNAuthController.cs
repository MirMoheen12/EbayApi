using EbayApi.AbaysideModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
namespace EbayApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EBayAuthNAuthController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAsync(string appId, string ruName)
        {

            using (var httpClient = new HttpClient())
            {
                var url = $"https://api.sandbox.ebay.com/ws/api.dll?callname=GetSessionID&appid={appId}&siteid=0&version=967&routing=default&RequesterCredentials=<eBayAuthToken%20xmlns=\"urn:ebay:apis:eBLBaseComponents\"><![CDATA[APP_AUTH_TOKEN]]></eBayAuthToken>&RuName={ruName}";
                var response = await httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    // handle error
                    throw new Exception("Failed to get session ID");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(responseContent);

                var sessionNode = xmlDoc.GetElementsByTagName("SessionID")[0];
               
                

            }
            return null;
        }

    

    }
}
