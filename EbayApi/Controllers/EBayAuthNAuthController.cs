using EbayApi.AbaysideModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
namespace EbayApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EBayAuthNAuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        public EBayAuthNAuthController(IConfiguration config)
        {

            _config = config;

        }

        [HttpGet]
        public async Task<string> GetSessionToken(string code)
        {
            string Authrizecode = _config.GetValue<string>("Authorizecode");
            string redirectUri = _config.GetValue<string>("RedericetUri");
            var authHeader = new AuthenticationHeaderValue("Basic", Authrizecode);
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = authHeader;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var tokenRequest = new HttpRequestMessage(HttpMethod.Post, "https://api.ebay.com/identity/v1/oauth2/token");
            tokenRequest.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "authorization_code",
                ["code"] = code,
                ["redirect_uri"] = redirectUri
            });
            var tokenResponse = await client.SendAsync(tokenRequest);
            if (!tokenResponse.IsSuccessStatusCode)
            {
                var errorResponse = await tokenResponse.Content.ReadAsStringAsync();
                return errorResponse;
            }
            var tokenResponseContent = await tokenResponse.Content.ReadAsStringAsync();
            var tokenJson = JObject.Parse(tokenResponseContent);
            var accessToken = (string)tokenJson["access_token"];

            return accessToken;

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
