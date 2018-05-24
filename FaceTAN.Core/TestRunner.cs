using FaceTAN.Core.ApiHandler;
using FaceTAN.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceTAN.Core.Data
{
    public enum API
    {
        Amazon,
        Azure
    }

    public class TestRunner
    {

        //List<BaseApiHandler> apiList = new List<BaseApiHandler>();
        Dictionary<API, BaseApiHandler> apiList = new Dictionary<API, BaseApiHandler>();
        private DataSet dsContext { get; set; }

        public TestRunner(DataSet context)
        {
            dsContext = context;

            // Setup API key stores
            ApiKeyStore amazonAccessKeys = new ApiKeyStore(new[] { "AKIAJJKYA2TLOIPHNNVA" });
            ApiKeyStore amazonPrivateKeys = new ApiKeyStore(new[] { "BBN6C1W3Lx0bo+mOgmD7xjlfstoA3qKA8ppIr38A" });
            ApiKeyStore azureKeys = new ApiKeyStore(new[] { "d6ba90bf1de54bf4a050c46eb1f73ab4" });

            apiList.Add(API.Amazon, new AmazonApiHandler(amazonAccessKeys, amazonPrivateKeys, dsContext, "testcollection"));
            apiList.Add(API.Azure, new AzureApiHandler(azureKeys, "https://australiaeast.api.cognitive.microsoft.com/face/v1.0", "", "test-person-group", dsContext));
        }

        public string RunTest(API targetAPI, List<object> sourceKeyArray, List<object> targetKeyArray)
        {
            dsContext.SourceImages.Clear();
            dsContext.TargetImages.Clear();

            if (sourceKeyArray != null)
            {
                sourceKeyArray.ForEach(sourceKey =>
                {
                    dsContext.SourceImages.Add(sourceKey.ToString(), dsContext.GetImage(sourceKey.ToString()));
                });
            }

            if (targetKeyArray != null)
            {
                targetKeyArray.ForEach(targetKey =>
                {
                    dsContext.TargetImages.Add(targetKey.ToString(), dsContext.GetImage(targetKey.ToString()));
                });
            }

            if(apiList.TryGetValue(targetAPI, out BaseApiHandler apiHandler))
            {
                Task apiRun = apiHandler.RunApi();
                apiRun.Wait();
                return apiHandler.ReturnJsonResults().MatchResults;
            }
            else
            {
                return "Can not find API Handler";
            }
        }

    }
}
