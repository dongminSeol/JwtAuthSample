using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuthenticationSample.Services
{
    public class JwtManager
    {

        private static readonly string Secret = "856FECBA3B06519C8DDDBC80BB080553"; 

        /// <summary>
        /// 토큰 생성
        /// </summary>
        /// <param name="username">이름</param>
        /// <param name="passwordHash">암호</param>
        /// <param name="expireHours">만료시간</param>
        /// <returns>token</returns>
        public static string GenerateToken(string username, string passwordHash, int expireHours = 12)
        {
            var symmetricKey = Convert.FromBase64String(Secret);
            var tokenHandler = new JwtSecurityTokenHandler();
            var now = DateTime.UtcNow;


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = now.AddHours(Convert.ToInt32(expireHours)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature),

                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Hash, passwordHash)
                })
            };


            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);

            return token;

        }

        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                {
                    return null;
                }

                var symmetricKey = Convert.FromBase64String(Secret);

                var validationParameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
                };

                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

                return principal;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 사용자 토큰의 정보가 유효 여부를 확인 합니다.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passwordHash"></param>
        /// <param name="jsonWebToken"></param>
        /// <returns></returns>
        public bool ValidateUser(string userName, string passwordHash, string jsonWebToken)
        {
            var principle = GetPrincipal(jsonWebToken);

            var identity = principle.Identity as ClaimsIdentity;

            if (identity == null)
            {
                return false;
            } 

            var usernNameClaim = identity.FindFirst(ClaimTypes.Name);
            var passwordHashClaim = identity.FindFirst(ClaimTypes.Hash);
            var userNameInJwt = usernNameClaim?.Value;
            var passwordHashInJwt = passwordHashClaim?.Value;

            return (userNameInJwt == userName && passwordHashInJwt == passwordHash);
        }

        public string GetUserNameFromToken(string jsonWebToken)
        {
            var principle = GetPrincipal(jsonWebToken);
            var identity = principle.Identity as ClaimsIdentity;
            var usernNameClaim = identity?.FindFirst(ClaimTypes.Name);

            return usernNameClaim?.Value;
        }


    }
}
