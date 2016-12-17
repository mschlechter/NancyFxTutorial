using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NancyFxTutorial.Web.Services
{
  public class WebTokenService : IWebTokenService
  {
    private string Issuer;
    private byte[] Secret;

    public WebTokenService(string issuer, string secret)
    {
      this.Issuer = issuer;
      this.Secret = Convert.FromBase64String(secret);
    }

    public string CreateToken(ClaimsIdentity claimsIdentity)
    {
      var signingKey = new SymmetricSecurityKey(Secret);
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
      var validationParameters = new TokenValidationParameters()
      {
        IssuerSigningKey = new SymmetricSecurityKey(Secret),
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