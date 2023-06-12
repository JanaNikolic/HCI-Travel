using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TravelApp.MVVM.View
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [ComVisible(true)]
    public class JavaScriptHelper
    {
        System.Windows.Window prozor;
        public JavaScriptHelper(System.Windows.Window w)
        {
            prozor = w;
        }

    }
}
