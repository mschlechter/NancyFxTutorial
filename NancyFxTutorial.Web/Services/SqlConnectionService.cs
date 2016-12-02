using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace NancyFxTutorial.Web.Services
{
  public class SqlConnectionService : IDbConnectionService
  {
    public IDbConnection OpenDbConnection()
    {
      var mainConnectionString = ConfigurationManager.ConnectionStrings["NancyFxTutorial.Web.Properties.Settings.MainConnectionString"].ConnectionString;

      var sqlConnection = new SqlConnection(mainConnectionString);
      sqlConnection.Open();

      return sqlConnection;
    }
  }
}