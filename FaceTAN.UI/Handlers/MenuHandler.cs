using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FaceTAN.UI.Handlers
{
    class MenuHandler : IContextMenuHandler
    {

        private const CefMenuCommand DEVTOOLS_ID = (CefMenuCommand)1001;
        private const CefMenuCommand CHROME_VERSION_ID = (CefMenuCommand)1002;

        public void OnBeforeContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
        {
            model.Remove(CefMenuCommand.Print);
            model.Remove(CefMenuCommand.ViewSource);

            model.AddItem(DEVTOOLS_ID, "Chrome Dev Tools");
            model.SetAccelerator(DEVTOOLS_ID, (int)Keys.D, false, true, false);

            model.AddItem(CHROME_VERSION_ID, "Chromium Version");
            model.SetAccelerator(CHROME_VERSION_ID, (int)Keys.V, false, true, false);
        }

        public bool OnContextMenuCommand(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, CefMenuCommand commandId, CefEventFlags eventFlags)
        {

            switch (commandId)
            {
                case DEVTOOLS_ID:
                    browser.ShowDevTools();
                    return false;
                case CHROME_VERSION_ID:
                    frame.LoadUrl("chrome://version/");
                    return false;
                case CefMenuCommand.Back:
                    browser.GoBack();
                    return false;
                case CefMenuCommand.Forward:
                    browser.GoForward();
                    return false;
            }

            return true;
        }

        public void OnContextMenuDismissed(IWebBrowser browserControl, IBrowser browser, IFrame frame)
        {
        }

        public bool RunContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback)
        {
            return false;
        }
    }
}

