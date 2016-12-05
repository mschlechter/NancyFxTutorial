using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace NancyFxTutorial.Web.Services
{
  // Intended use:
  // Be able to inject SqlConnectionService using a request scope and let it
  // be disposed when the request / response cycle is finished.
  public class SqlConnectionService : IDbConnectionService 
  {
    private IDbConnection DbConnection;

    public void Dispose()
    {
      // Close existing IDbConnection. THIS IS IMPORTANT!
      DbConnection?.Dispose();
    }

    public IDbConnection GetDbConnection()
    {
      // Open existing IDbConnection
      if (DbConnection != null) return DbConnection;

      // Create new IDbConnection if we don't have one
      var mainConnectionString = ConfigurationManager.ConnectionStrings["NancyFxTutorial.Web.Properties.Settings.MainConnectionString"].ConnectionString;

      DbConnection = new SqlConnection(mainConnectionString);
      DbConnection.Open();

      return DbConnection;
    }
  }
}