using NancyFxTutorial.Web.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NancyFxTutorial.Web.Services
{
  public interface IAuthenticationService
  {
    AuthLogon GetLogonByCredentials(string username, string password);
  }
}
