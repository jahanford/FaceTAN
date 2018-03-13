using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceTAN.Core.ApiHandler
{
    public abstract class BaseApiHandler
    {
        public string ApiName;
        public abstract Task RunApi();
        public abstract void ExportResults(string outputDirectory);
    }
}
