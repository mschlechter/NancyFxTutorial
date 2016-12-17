using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;


namespace NancyFxTutorial.Web.Services
{
  public class WebTokenService : IWebTokenService
  {
    private string Issuer;
    private string Secret;

    public WebTokenService(string issuer, string secret)
    {
      this.Issuer = issuer;
      this.Secret = secret;
    }

    public string CreateToken(ClaimsIdentity claimsIdentity)
    {
      byte[] secretBytes = Convert.FromBase64String(Secret);

      var signingKey = new SymmetricSecurityKey(secretBytes);
      var signingCredentials = new SigningCredentials(signingKey,
          SecurityAlgorithms.HmacSha256Signature);

      var securityTokenDescriptor = new SecurityTokenDescriptor()
      {
        Issuer = this.Issuer,
        Subject = claimsIdentity,
        SigningCredentials = signingCredentials,
        Expires = DateTime.Now.AddMinutes(15)
      }; // Token is 15 minuten geldig

      var tokenHandler = new JwtSecurityTokenHandler();

      var plainToken = tokenHandler.CreateToken(securityTokenDescriptor);
      var signedAndEncodedToken = tokenHandler.WriteToken(plainToken);

      return signedAndEncodedToken;
    }

    public ClaimsPrincipal ValidateToken(string token)
    {
      byte[] secretBytes = Convert.FromBase64String(Secret);

      var validationParameters = new TokenValidationParameters()
      {
        IssuerSigningKey = new SymmetricSecurityKey(secretBytes),
        ValidIssuer = this.Issuer,
        ValidateLifetime = true,
        ValidateAudience = false,
        ValidateIssuer = true,
        ValidateIssuerSigningKey = true
      };

      var tokenHandler = new JwtSecurityTokenHandler();
      SecurityToken validatedToken = null;

      var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
      return principal;
    }
  }
}