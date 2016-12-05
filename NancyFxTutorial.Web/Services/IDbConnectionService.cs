using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace NancyFxTutorial.Web.Services
{
  public interface IDbConnectionService : IDisposable 
  {
    IDbConnection GetDbConnection();
  }
}