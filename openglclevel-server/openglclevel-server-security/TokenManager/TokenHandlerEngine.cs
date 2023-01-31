using Microsoft.IdentityModel.Tokens;
using openglclevel_server_models.API.Security;
using openglclevel_server_models.Security;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace openglclevel_server_security.TokenManager
{
    public class TokenHandlerEngine
    {

        private readonly ISecurityKeys _securityKeysValues;
        public TokenHandlerEngine(ISecurityKeys securityKeysValues)
        {
            _securityKeysValues = securityKeysValues;
        }


        public TokenResultModel GenerateToken(List<KeyValuePair<string, string>> tokenInfo, Guid userID)
        {
            var permClaims = new List<Claim>();

            tokenInfo.ForEach(fe => permClaims.Add(new Claim(fe.Key, fe.Value)));

            //permClaims.Add(new Claim("companyId", userInfo.CompanyId.ToString()));
            //permClaims.Add(new Claim("email", userInfo.Email));


            var tokenAudience = "openglclevel-app";

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_securityKeysValues.JWT_PrivateKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var expiration = permClaims.Any(a=> a.Type == "allowDeleteActionToken" && a.Value == "true") ? 
                DateTime.Now.AddSeconds(30) : DateTime.Now.AddDays(365);

            var token = new JwtSecurityToken("openglclevel-app",
              tokenAudience, claims: permClaims,
              null,
              expires: expiration,
              signingCredentials: credentials);



            //TODO: get time from token object
            //expiration = !generateSpecialToken ? DateTime.Now.AddMinutes(60) : DateTime.Now.AddSeconds(30);

            return new TokenResultModel { Token = new JwtSecurityTokenHandler().WriteToken(token), UserID = userID };

        }

        public static JwtSecurityToken GetTokenDataByStringValue(string rawToken)
        {
            var token = rawToken;
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            return jwtSecurityToken;
        }
    }
}
