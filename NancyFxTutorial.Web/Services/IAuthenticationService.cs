using NancyFxTutorial.Web.Core;
using NancyFxTutorial.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NancyFxTutorial.Web.Services
{
  public interface IAuthenticationService
  {
    AuthenticationLogon GetLogonByCredentials(string username, string password);
  }
}
