using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NancyFxTutorial.Web.Core;
using System.Data;
using NancyFxTutorial.Web.Models;

namespace NancyFxTutorial.Web.Services
{
  public class AuthenticationService : IAuthenticationService
  {
    IDbConnectionService DbConnectionService;

    public AuthenticationService(IDbConnectionService dbConnectionService)
    {
      this.DbConnectionService = dbConnectionService;
    }

    public AuthenticationLogon GetLogonByCredentials(string username, string password)
    {
      using (IDbConnection dbConnection = DbConnectionService.OpenDbConnection())
      {
        var user = dbConnection.Query<AuthenticationLogon>(
          "SELECT * FROM [User] WHERE Username = @Username AND Password = @Password",
          new { Username = username, Password = password }).SingleOrDefault();

        return user;
      }
    }
  }
}