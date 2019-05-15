using CefSharp;

namespace Huawei.SCCMPlugin.FusionDirector.PluginUI.FDScheme
{
    /// <summary>
    /// ����CefSharp��Scheme����
    /// </summary>
    public class CefSharpSchemeHandlerFactory : ISchemeHandlerFactory
    {
        public const string SchemeName = "huawei";
        public const string SchemeNameTest = "test";

        public IResourceHandler Create(IBrowser browser, IFrame frame, string schemeName, IRequest request)
        {
            if (schemeName == SchemeName && request.Url.EndsWith("CefSharp.Core.xml", System.StringComparison.OrdinalIgnoreCase))
            {
                //Display the debug.log file in the browser
                return ResourceHandler.FromFileName("CefSharp.Core.xml", ".xml");
            }
            return new CefSharpSchemeHandler();
        }
    }
}