using System;
using System.Data;

namespace NancyFxTutorial.Web.Services
{
  public interface IDbConnectionService : IDisposable 
  {
    IDbConnection GetDbConnection();
  }
}