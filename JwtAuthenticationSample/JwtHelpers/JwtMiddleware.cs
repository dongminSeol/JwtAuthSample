using JwtAuthenticationSample.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthenticationSample.JwtHelpers
{
    public class JwtMiddleware 
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, IUserData userService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                attachUserToContext(context, userService, token);

            await _next(context);
        }

        private void attachUserToContext(HttpContext context, IUserData userService, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Convert.FromBase64String(_appSettings.Secret);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    //만료 시간 필요
                    RequireExpirationTime = true,
                    //유효성 검사 Issuer Signing Key
                    ValidateIssuerSigningKey = true,
                    // IssuerSigningKey
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // 토큰이 토큰 만료 시간에 정확히 만료되도록 0으로 클럭화(5분 후 만료)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                var userId = jwtToken.Claims.First(x => x.Type == "Name").Value;
                var userHash = jwtToken.Claims.First(x => x.Type == "Hash").Value;

                // 사용자를 성공적인 jwt 유효성 검사에 대한 컨텍스트에 연결
                //context.Items["User"] = userService.GetByUser(userId, userHash);
            }
            catch
            {
                // jwt 유효성 검사에 실패해도 아무 조치도 취하지 않음.
                // 사용자가 컨텍스트에 연결되어 있지 않으므로 요청이 보안 경로에 액세스할 수 없음
            }
        }
    }
}
