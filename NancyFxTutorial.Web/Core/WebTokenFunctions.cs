using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Web;


namespace NancyFxTutorial.Web.Core
{
  public class WebTokenFunctions
  {
    public static ClaimsIdentity CreateClaimIdentity(string userId, string userName, string role)
    {
      var claimsIdentity = new ClaimsIdentity(new List<Claim>()
      {
          new Claim(ClaimTypes.NameIdentifier, userId),
          new Claim(ClaimTypes.Name, userName),
          new Claim(ClaimTypes.Role, role)
      }, "Custom");

      return claimsIdentity;
    }

    public static string CreateToken(ClaimsIdentity claimsIdentity, string issuerName, string secret)
    {
      byte[] secretBytes = Convert.FromBase64String(secret);
      return CreateToken(claimsIdentity, issuerName, secretBytes);
    }

    public static string CreateToken(ClaimsIdentity claimsIdentity, string issuerName, byte[] secret)
    {
      var signingKey = new SymmetricSecurityKey(secret);
      var signingCredentials = new SigningCredentials(signingKey,
          SecurityAlgorithms.HmacSha256Signature);

      var securityTokenDescriptor = new SecurityTokenDescriptor()
      {
        Issuer = issuerName,
        Subject = claimsIdentity,
        SigningCredentials = signingCredentials,
        Expires = DateTime.Now.AddMinutes(15)
      }; // Token is 15 minuten geldig

      var tokenHandler = new JwtSecurityTokenHandler();

      var plainToken = tokenHandler.CreateToken(securityTokenDescriptor);
      var signedAndEncodedToken = tokenHandler.WriteToken(plainToken);

      return signedAndEncodedToken;
    }

    public static ClaimsPrincipal ValidateToken(string token, string issuerName, string secret)
    {
      byte[] secretBytes = Convert.FromBase64String(secret);
      return ValidateToken(token, issuerName, secretBytes);
    }

    public static ClaimsPrincipal ValidateToken(string token, string issuerName, byte[] secret)
    {
      var validationParameters = new TokenValidationParameters()
      {
        IssuerSigningKey = new SymmetricSecurityKey(secret),
        ValidIssuer = issuerName,
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

    public static string CreateSecret() // Om er eentje aan te maken
    {
      using (var cryptoProvider = new RNGCryptoServiceProvider())
      {
        byte[] secretKeyByteArray = new byte[32]; // 256 bit
        cryptoProvider.GetBytes(secretKeyByteArray);
        return Convert.ToBase64String(secretKeyByteArray);
      }
    }
  }
}