using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JwtAuthenticationSample.Services;
using JwtAuthenticationSample.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthenticationSample.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {

        // POST /api/account/signin
        [HttpPost]
        [Route("signin")]
        [AllowAnonymous]
        public IActionResult LoginUser([FromBody] LoginUserBindingModel loginUserModel)
        {


            //저장소에서 
            string admin_name = "admin";
            string admin_password = "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918";

            try
            {
                if (loginUserModel != null &&
                    admin_name     == loginUserModel.UserName &&
                    admin_password == PasswordEncoder.Encode(loginUserModel.Password))
                {

                    return Ok(new
                    {
                        Token    = JwtManager.GenerateToken(loginUserModel.UserName, loginUserModel.Password),
                        UserName = loginUserModel.UserName,
                        Password = PasswordEncoder.Encode(loginUserModel.Password)
                    });
                }

                return BadRequest();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }

        // POST /api/account/signins
        [HttpGet]
        [Route("publicTest")]
        public IActionResult LoginUsers()
        {

            string admin_name = "admin";
            string admin_password = "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918";


            return Ok(new
            {
                Token = JwtManager.GenerateToken(admin_name, "admin"),
                UserName = "관리자",
                Password = admin_password
            });

        }


    }
}