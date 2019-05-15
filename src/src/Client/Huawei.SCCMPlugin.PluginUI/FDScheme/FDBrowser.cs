using System;
using System.Drawing;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using Huawei.SCCMPlugin.FusionDirector.PluginUI.Interface;
using Newtonsoft.Json;
using CommonUtil;

namespace Huawei.SCCMPlugin.FusionDirector.PluginUI.FDScheme
{
    /// <summary>
    /// 定义FD Browser
    /// </summary>
    public partial class FDBrowser : ChromiumWebBrowser, ICefBrowser
    {
        public FDBrowser(string url) : base("huawei://fd-plugin-webapp/" + url)
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
            Name = "FDBrowser";
            MinimumSize = new Size(20, 20);
            LoadError += WebBrowser_LoadError;
            ConsoleMessage += OnBrowserConsoleMessage;
            LoadingStateChanged += OnLoadingStateChanged;
        }

        #region Methods
        /// <summary>
        /// Executes the script.
        /// </summary>
        /// <param name="function">The function.</param>
        /// <param name="data">The data.</param>
        public void ExecuteScript(string function, object data)
        {
            this.ExecuteScriptAsync($"{function}({JsonUtil.SerializeObject(data)})");
        }

        /// <summary>
        /// Executes the script.
        /// </summary>
        /// <param name="jsCode">The js code.</param>
        public void ExecuteScript(string jsCode)
        {
            this.ExecuteScriptAsync(jsCode);
        }

        /// <summary>
        /// Registers the js object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="boundName">Name of the bound.</param>
        public void RegisterJsObject<T>(string boundName) where T : new()
        {
            this.RegisterJsObject(boundName, new T(), camelCaseJavascriptNames: true);
        }

        #endregion

        #region Events

        /// <summary>
        /// on console message(Source,Line,Message)
        /// </summary>
        public Action<string, int, string> OnConsoleMessage;

        private void OnBrowserConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            OnConsoleMessage?.Invoke(e.Source, e.Line, e.Message);
        }


        /// <summary>
        ///  on load error(failedUrl, errorCode, errorText)
        /// </summary>
        public Action<string, string, string> OnLoadError;

        /// <summary>
        /// Webs the browser_ load error.
        /// </summary>
        /// <param name="sender">The sender.</param>
        private void WebBrowser_LoadError(object sender, LoadErrorEventArgs e)
        {
            OnLoadError?.Invoke(e.FailedUrl, e.ErrorCode.ToString(), e.ErrorText);
        }

        public Action OnLoadFinish { get; set; }

        private void OnLoadingStateChanged(object sender, LoadingStateChangedEventArgs args)
        {
            if (args.IsLoading == false)
            {
                OnLoadFinish?.Invoke();
            }
        }

        #endregion
    }
}
