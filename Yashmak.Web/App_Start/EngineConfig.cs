namespace Yashmak.Web
{
    using System.Web.Mvc;

    public class EngineConfig
    {
        public static void RegisterEngines(ViewEngineCollection engines)
        {
            engines.Clear();
            engines.Add(new RazorViewEngine());
        }
    }
}