using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NancyFxTutorial.Web.Models
{
  public class AuthenticationRequest
  {
    public string Naam { get; set; }
    public string Wachtwoord { get; set; }
  }
}