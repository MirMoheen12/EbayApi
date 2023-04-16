using EbayApi.AbaysideModels;
using EbayApi.DbModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace EbayApi.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class AccountSideController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;
        public AccountSideController(IConfiguration config,ApplicationDbContext context)
        {
            this._config = config;
            this._context = context;
        }
       
        private AllUsers ValidateUser(AllUsers allUsers)
        {
            var data = _context.AllUsers.Where(x => x.UserEmail == allUsers.UserEmail && x.UserPassword == allUsers.UserPassword).FirstOrDefault();
            return data;
        }
        private string GenerateToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:key"]));
            var crdentials=new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], null, expires: DateTime.Now.AddMinutes(5), signingCredentials: crdentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        [AllowAnonymous]
        [HttpPost]
        public string Login(AllUsers users)
        {
            string response = null;
            var User=ValidateUser(users);
            if(User!=null)
            {
                var token=GenerateToken();
                response = token;
            }
            return response;
        }

   
    }
}
