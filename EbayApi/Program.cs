using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = "eBay";
})
.AddCookie()
.AddOAuth("eBay", options =>

{
    options.ClientId = "RomanAzi-Inventor-SBX-4d7513f64-220e8727";
    options.ClientSecret = "SBX-d7513f64924e-7ad9-4a67-bddd-1437";
    options.CallbackPath = new PathString("/signin-ebay");
    options.AuthorizationEndpoint = "https://auth.sandbox.ebay.com/oauth2/authorize";
    options.TokenEndpoint = "https://auth.sandbox.ebay.com/oauth2/token";
    options.Scope.Add("https://api.sandbox.ebay.com/oauth/api_scope");
    options.SaveTokens = true;
    options.ClaimActions.MapJsonKey("urn:ebay:profilepicture", "thumbnailUrl");
    options.Events = new OAuthEvents
    {
        OnCreatingTicket = async context =>
        {
            var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
            var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
            response.EnsureSuccessStatusCode();
            var user = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
            context.RunClaimActions(user);
        }
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
