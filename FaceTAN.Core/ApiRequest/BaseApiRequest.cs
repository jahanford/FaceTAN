using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceTAN.Core.ApiRequest
{
    public abstract class BaseApiRequest
    {
        public BaseApiRequest(string endpointUrl)
        {
            EndpointUrl = endpointUrl;
        }

        private string EndpointUrl { get; }
    }
}
