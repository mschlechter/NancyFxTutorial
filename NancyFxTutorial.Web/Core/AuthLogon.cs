using Nancy.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NancyFxTutorial.Web.Core
{
  public class AuthLogon : IUserIdentity
  {
    public IEnumerable<string> Claims
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public int UserID { get; set; }
    public string UserName { get; set; }
    public string Role { get; set; }

    public string VolledigeNaam { get; set; }
  }
}