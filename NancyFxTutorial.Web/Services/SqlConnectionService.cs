using System.Data;
using System.Data.SqlClient;

namespace NancyFxTutorial.Web.Services
{
  // De SqlConnectionService bestaat gedurende een gehele request.
  // Hierdoor kan dezelfde SqlConnection door meerdere services gedeeld
  // worden (uiteraard niet tegelijkertijd)
  public class SqlConnectionService : IDbConnectionService 
  {
    private IDbConnection DbConnection;
    private string ConnectionString;

    public SqlConnectionService(string connectionString)
    {
      this.ConnectionString = connectionString;
    }

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
      DbConnection = new SqlConnection(ConnectionString);
      DbConnection.Open();

      return DbConnection;
    }
  }
}