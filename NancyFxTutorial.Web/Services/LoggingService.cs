﻿namespace NancyFxTutorial.Web.Services
{
  public class LoggingService : ILoggingService
  {
    IDbConnectionService DbConnectionService;

    public LoggingService(IDbConnectionService dbConnectionService)
    {
      this.DbConnectionService = dbConnectionService;
    }

    public void LogMessage(string message)
    {
      var dbConnection = DbConnectionService.GetDbConnection();

      // Voeg hier code toe die de IDbConnection nodig heeft
    }
  }
}