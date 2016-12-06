using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace NancyFxTutorial.Web.Services
{
  // De SqlConnectionService bestaat gedurende een gehele request.
  // Hierdoor kan dezelfde SqlConnection door meerdere services gedeeld
  // worden (uiteraard niet tegelijkertijd)
  public class SqlConnectionService : IDbConnectionService 
  {
    private IDbConnection DbConnection;

    public void Dispose()
    {
      // Sluit de openstaande SQL verbinding
      DbConnection?.Dispose();
    }

    public IDbConnection GetDbConnection()
    {
      // Geeft de bestaande SQL verbinding terug
      if (DbConnection != null) return DbConnection;

      // Maak een nieuwe SQL verbinding
      var mainConnectionString = ConfigurationManager.ConnectionStrings["NancyFxTutorial.Web.Properties.Settings.MainConnectionString"].ConnectionString;

      DbConnection = new SqlConnection(mainConnectionString);
      DbConnection.Open();

      return DbConnection;
    }
  }
}