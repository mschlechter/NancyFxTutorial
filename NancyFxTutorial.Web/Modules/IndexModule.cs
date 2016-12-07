using Nancy;

namespace NancyFxTutorial.Web.Modules
{
  public class IndexModule : NancyModule
  {
    public IndexModule()
    {
      Get["/"] = _ => View["Index"];
    }
  }
}