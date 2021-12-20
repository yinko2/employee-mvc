using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using EmployeeMVC.Util;
using EmployeeMVC.Models;
using EmployeeMVC.Repository;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace EmployeeMVC.Controllers
{
    [Route("api/[controller]")]
    public class BaseAPIController : Controller
    {
        // public TokenData _tokenData = new TokenData();
        public readonly IRepositoryWrapper _repositoryWrapper;
        public IConfiguration _configuration;

        public BaseAPIController(IRepositoryWrapper repositoryWrapper, IConfiguration configuration)
        {
            _repositoryWrapper = repositoryWrapper;
            _configuration = configuration;            
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            // await setDefaultDataFromToken();
        }

        // public async Task setDefaultDataFromToken()
        // {
        //     try
        //     {
        //         string access_token = "";
        //         var hdtoken = Request.Headers["Authorization"];
        //         if (hdtoken.Count > 0)
        //         {
        //             access_token = hdtoken[0];
        //             access_token = access_token.Replace("Bearer ", "");
        //             var handler = new JwtSecurityTokenHandler();
        //             var tokenS = handler.ReadToken(access_token) as JwtSecurityToken;
        //             _tokenData = Globalfunction.GetTokenData(tokenS);
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine(ex.Message);
        //         await _repositoryWrapper.EventLog.Error("Read token error", ex.Message, "Base >> setDefaultDataFromToken");
        //     }
        // }   
    }
}