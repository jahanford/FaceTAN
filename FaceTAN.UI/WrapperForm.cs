using CefSharp;
using CefSharp.WinForms;
using FaceTAN.UI.Handlers;
using System;
using System.Drawing;
using System.Windows.Forms;
using FaceTAN.Core;
using FaceTAN.Core.Data;

namespace FaceTAN.UI
{
    public partial class WrapperForm : Form
    {
        static string onLoadURL = string.Format(Application.StartupPath + @"..\..\..\..\" + @"\html-resources\index.html");
        private readonly ChromiumWebBrowser chromium;     

        public WrapperForm()
        {
            //this.FormBorderStyle = FormBorderStyle.None;
            this.HScroll = false;
            this.VScroll = false;

            this.StartPosition = FormStartPosition.CenterScreen;
            this.ShowIcon = false;

            InitializeComponent();
            this.Text = "FaceTan UI";
            this.MinimumSize = new Size(1000, 700);

            CefInstance CefContext = new CefInstance();

            chromium = new ChromiumWebBrowser(onLoadURL)
            {
                Name = Guid.NewGuid().ToString(),
                Dock = DockStyle.Fill,
                MenuHandler = new MenuHandler(),
                Top = 0
            };
            this.Controls.Add(chromium);

            var formHandler = new FormProvider(this);

            //Register DataSet Object to boundDataSet Javascript Object
            chromium.JavascriptObjectRepository.Register("boundDataSet", new DataSet("capstone-dataset", "AKIAJJKYA2TLOIPHNNVA", "BBN6C1W3Lx0bo+mOgmD7xjlfstoA3qKA8ppIr38A", 10), true);
            
            IntializeEventHandlers();
        }

        public class CefInstance
        {

            private readonly CefSettings defaultSettings = new CefSettings()
            {
                JavascriptFlags = "",
                RemoteDebuggingPort = 8080,
                ProductVersion = "0.01",
                WindowlessRenderingEnabled = true
            };

            public CefInstance(CefSettings settings = null)
            {
                CefSharpSettings.LegacyJavascriptBindingEnabled = true;
                if (settings == null) settings = defaultSettings;
                Cef.Initialize(settings);
            }
        }

        const int WM_NCHITTEST = 0x0084;
        const int HTCLIENT = 1;
        const int HTCAPTION = 2;
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            switch (m.Msg)
            {
                case WM_NCHITTEST:
                    if (m.Result == (IntPtr)HTCLIENT)
                    {
                        m.Result = (IntPtr)HTCAPTION;
                    }
                    break;
            }
        }
        
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style |= 0x40000;
                return cp;
            }
        }

        private void IntializeEventHandlers()
        {
            this.Load += Wrapper_Load;
        }

        private void Wrapper_Load(object sender, EventArgs e)
        {
            this.Controls.Add(chromium);
            Console.WriteLine("Wrapper loaded!");
        }

        

        interface IFormProvider
        {
            bool SetFormOpacity(int opacity);
        }

        public class FormProvider : IFormProvider
        {
            WrapperForm FormContext;

            public FormProvider(WrapperForm form)
            {
                FormContext = form;
            }

            public bool SetFormOpacity(int opacity)
            {
                FormContext.Opacity = opacity;
                return true;
            }
        }

        interface ICefProvider
        {
            void Error(string e);

            bool LoadURL(string URL);

            string CefSharpVersion();

            string CefVersion();

            string BrowserName();
        }

        class CefProvider : ICefProvider
        {

            ChromiumWebBrowser BrowserScope;
            WrapperForm FormContext;

            public CefProvider(ChromiumWebBrowser browser, WrapperForm form)
            {
                BrowserScope = browser;
                FormContext = form;
            }

            public bool SetFormOpacity(int opacity)
            {
                FormContext.Opacity = opacity;
                return true;
            }

            public string CefSharpVersion()
            {
                return Cef.CefSharpVersion;
            }

            public string CefVersion()
            {
                return Cef.CefVersion;
            }

            public string BrowserName()
            {
                return BrowserScope.Name;
            }

            public void Error(string e)
            {
                throw new Exception(e);
            }

            public bool LoadURL(string URL)
            {
                BrowserScope.Load(URL);
                return true;
            }

        }

    }
    
}
