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
        private IUserData _userData;

        public AuthorizationController(IUserData userData)
        {
            _userData = userData;
        }

        // POST /api/account/signin
        [HttpPost]
        [Route("signin")]
        [AllowAnonymous]
        public IActionResult LoginUser([FromBody] LoginUserBindingModel model) 
        {


            /* 테스트 기본 정보
               아이디 : Admin
               암호   : Admin */
            try
            {
                var user = _userData.GetByUser(model.UserName, PasswordEncoder.Encode(model.Password));

                if (user != null && user.PasswordHash == PasswordEncoder.Encode(model.Password))
                {
                    return Ok(new
                    {
                        token = _userData.GenerateToken(model.UserName, model.Password),
                        username = model.UserName,
                        passwordhash = PasswordEncoder.Encode(model.Password)
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
                Token = _userData.GenerateToken(admin_name, "admin"),
                UserName = "관리자",
                Password = admin_password
            });

        }


    }
}