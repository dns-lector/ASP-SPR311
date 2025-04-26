using ASP_SPR311.Data;
using ASP_SPR311.Data.Entities;
using ASP_SPR311.Middleware;
using ASP_SPR311.Models;
using ASP_SPR311.Models.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Text;
using System.Text.Json;

namespace ASP_SPR311.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class ApiUserController(DataAccessor dataAccessor) : ControllerBase
    {
        private readonly DataAccessor _dataAccessor = dataAccessor;

        [HttpGet("jti")]
        public RestResponse Authenticate()
        {
            var res = new RestResponse
            {
                Service = "API User Authentication",
                DataType = "object",
                CacheTime = 600,
            };
            try
            {
                res.Data = _dataAccessor.Authenticate(Request);
            }
            catch (Win32Exception ex)
            {
                res.Status = new()
                {
                    IsOk = false,
                    Code = ex.HResult,
                    Phrase = ex.Message
                };
                res.Data = null;
            }
            return res;
        }
        
        [HttpGet]
        public RestResponse AuthenticateJwt()
        {
            var res = new RestResponse
            {
                Service = "API User Authentication",
                DataType = "object",
                CacheTime = 600,
            };
            try
            {
                String header = Base64UrlTextEncoder.Encode(
                    Encoding.UTF8.GetBytes(
                        "{\"alg\": \"HS256\",\"typ\": \"JWT\"}"));

                String payload = Base64UrlTextEncoder.Encode( 
                    Encoding.UTF8.GetBytes(
                        JsonSerializer.Serialize(
                            _dataAccessor.Authenticate(Request))));

                String data = header + "." + payload;

                String signature = Base64UrlTextEncoder.Encode(
                    System.Security.Cryptography.HMACSHA256.HashData(
                        Encoding.UTF8.GetBytes("secret"),
                        Encoding.UTF8.GetBytes(data)));

                res.Data = data + "." + signature;
            }
            catch(Win32Exception ex)
            {
                res.Status = new()
                {
                    IsOk = false,
                    Code = ex.HResult,
                    Phrase = ex.Message
                };
                res.Data = null;
            }
            return res;
        }

        [HttpGet("profile")]
        public RestResponse Profile()
        {
            var res = new RestResponse
            {
                Service = "API User Profile",
                DataType = "object",
                CacheTime = 600,
            };
            if(HttpContext.User.Identity?.IsAuthenticated ?? false)
            {
                res.Data = (HttpContext.Items["AccessToken"] as AccessToken)?.User;
            }
            else
            {
                res.Status = new()
                {
                    IsOk = false,
                    Code = 401,
                    Phrase = HttpContext.Items[nameof(AuthTokenMiddleware)]?.ToString() ?? ""
                };
                res.Data = null;
            }
            return res;
        }


        [HttpPost]
        public RestResponse SignUp(UseApiSignupFormModel model)
        {
            var res = new RestResponse
            {
                Service = "API User Registration",
                DataType = "object",
                CacheTime = 0,
                Data = model
            };
            return res;
        }
    }
}
