namespace Yashmak.Web
{
    using System.Web.Mvc;

    public class EngineConfig
    {
        public static void RegisterEngines()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
        }
    }
}