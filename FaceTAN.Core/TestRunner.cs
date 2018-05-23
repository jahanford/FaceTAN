using FaceTAN.Core.ApiHandler;
using FaceTAN.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceTAN.Core.Data
{
    public class TestRunner
    {

        List<BaseApiHandler> apiList = new List<BaseApiHandler>();
        Dictionary<string, ApiResults> testResults = new Dictionary<string, ApiResults>();
        private DataSet dsContext { get; set; }

        public TestRunner(DataSet context)
        {
            dsContext = context;

            // Setup API key stores
            ApiKeyStore amazonAccessKeys = new ApiKeyStore(new[] { "AKIAJJKYA2TLOIPHNNVA" });
            ApiKeyStore amazonPrivateKeys = new ApiKeyStore(new[] { "BBN6C1W3Lx0bo+mOgmD7xjlfstoA3qKA8ppIr38A" });
            ApiKeyStore azureKeys = new ApiKeyStore(new[] { "d6ba90bf1de54bf4a050c46eb1f73ab4" });

            apiList.Add(new AmazonApiHandler(amazonAccessKeys, amazonPrivateKeys, dsContext, "testcollection"));
           //apiList.Add(new AzureApiHandler(azureKeys, "https://australiaeast.api.cognitive.microsoft.com/face/v1.0", "", "test-person-group", dsContext));
        }

        public string RunTest(string testGuid, List<object> sourceKeyArray, List<object> targetKeyArray)
        {
            dsContext.SourceImages.Clear();
            dsContext.TargetImages.Clear();
            testResults.Clear();

            Console.Write("Running Test Guid: " + testGuid);

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

            apiList.ForEach((api) =>
            {
                Task apiRun = api.RunApi();
                apiRun.Wait();
                testResults.Add(api.ApiName, api.ReturnJsonResults());
            });

            if (testResults.TryGetValue("Amazon Rekognition", out ApiResults value)) { return value.MatchResults; }
            return "TEST FAILED :(";
        }
    }
}
