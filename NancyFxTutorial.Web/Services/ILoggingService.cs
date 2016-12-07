using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NancyFxTutorial.Web.Services
{
  public interface ILoggingService
  {
    void LogMessage(string message);
  }
}