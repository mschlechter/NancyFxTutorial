using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NancyFxTutorial.Web.Core
{
  public class AppUtils
  {
    // Deze kun je genereren met de WebTokenFunctions.CreateSecret functie
    public static string SecretApiKey { get; set; } = "+a8tTwAuErCTfvPYtGeaS/dMJmxx9IlbmlOt8QnHAqY=";

    public static string Issuer { get; set; } = "NancyFxTutorial";

  }
}