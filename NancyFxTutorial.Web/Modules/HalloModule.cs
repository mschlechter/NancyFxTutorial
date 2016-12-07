using Nancy;

namespace NancyFxTutorial.Web.Modules
{
  public class HalloModule : NancyModule
  {
    public HalloModule()
    {
      Get["/hallo"] = parameters => "Hallo wereld";
    }
  }
}