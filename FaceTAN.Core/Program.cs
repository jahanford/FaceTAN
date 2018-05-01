using FaceTAN.Core.ApiHandler;
using FaceTAN.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceTAN.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get desired data set size from the user
            Console.Write("Enter the desired target data set size: ");
            int dataSetSize;
            if (int.TryParse(Console.ReadLine(), out dataSetSize) == false)
            {
                dataSetSize = 8;
                Console.WriteLine("Unable to parse input. Using default data set size of {0}", dataSetSize.ToString());
            }

            // Setup API key stores
            ApiKeyStore amazonAccessKeys = new ApiKeyStore(new[] { "AKIAJJKYA2TLOIPHNNVA" });
            ApiKeyStore amazonPrivateKeys = new ApiKeyStore(new[] { "BBN6C1W3Lx0bo+mOgmD7xjlfstoA3qKA8ppIr38A" });
            ApiKeyStore azureKeys = new ApiKeyStore(new[] { "d6ba90bf1de54bf4a050c46eb1f73ab4" });
            ApiKeyStore animetricsKeys = new ApiKeyStore(new[] { "UINlGk5i5lmsha6RTFLEbd1XL65Ap1Y5kq2jsnuaYrGkAyQcCg" });
            ApiKeyStore lambdaKeys = new ApiKeyStore(new[] { "UINlGk5i5lmsha6RTFLEbd1XL65Ap1Y5kq2jsnuaYrGkAyQcCg" });

            // Setup DataSet
            DataSet dataSet = new DataSet("capstone-dataset", "AKIAJJKYA2TLOIPHNNVA", "BBN6C1W3Lx0bo+mOgmD7xjlfstoA3qKA8ppIr38A", dataSetSize);

            // Setup SubSets
            List<SubSet> subSets = new List<SubSet>();
            subSets.Add(new SubSet(dataSet.TargetImages.Keys.ToList(), dataSet.SourceImages.Keys.ToList())); // Add all dataset images to a subset

            //Setup the various APIs
            List<BaseApiHandler> apiList = new List<BaseApiHandler>();
            //apiList.Add(new AmazonApiHandler(amazonAccessKeys, amazonPrivateKeys, dataSet, "testcollection"));
            //apiList.Add(new AzureApiHandler(azureKeys, "https://australiaeast.api.cognitive.microsoft.com/face/v1.0", "", "test-person-group", dataSet));
            apiList.Add(new AnimetricsApiHandler(animetricsKeys, "https://animetrics.p.mashape.com/", "test_gallery", dataSet));
            //apiList.Add(new LambdaApiHandler(lambdaKeys, "https://lambda-face-recognition.p.mashape.com/", dataSet));

            subSets.ForEach((subset) =>
            {
                Console.WriteLine("Running APIs for subset %1", subset.SubSetId);
                apiList.ForEach((api) =>
                { 
                    Console.WriteLine("Testing {0} API.", api.ApiName);
                    Task apiRun = api.RunApi();
                    apiRun.Wait();
                    Console.WriteLine("API {0} Complete. Exporting results...", api.ApiName);
                    api.ExportResults("D:\\Api Output\\" + DateTime.Now.Date.ToShortDateString() + "-" + dataSetSize);
                    Console.WriteLine("Export Complete.", api.ApiName);
                });
            });

            Console.WriteLine("Press Any Key To Continue...");
            Console.ReadKey();
        }
    }
}
