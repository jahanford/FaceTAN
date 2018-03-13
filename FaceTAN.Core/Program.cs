using FaceTAN.Core.ApiHandler;
using FaceTAN.Core.Data;
using System;
using System.Collections.Generic;
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

            // Setup DataSet
            DataSet dataSet = new DataSet("capstone-dataset", "AKIAJJKYA2TLOIPHNNVA", "BBN6C1W3Lx0bo+mOgmD7xjlfstoA3qKA8ppIr38A", dataSetSize);

            //Setup the various APIs
            List<BaseApiHandler> apiList = new List<BaseApiHandler>();
            apiList.Add(new AmazonApiHandler("AKIAJJKYA2TLOIPHNNVA", "BBN6C1W3Lx0bo+mOgmD7xjlfstoA3qKA8ppIr38A", dataSet, "testcollection"));
            apiList.Add(new AzureApiHandler("3ab30cd064c04013bd868bf7d7c8a2f4", "https://westcentralus.api.cognitive.microsoft.com/face/v1.0", "", "test-person-group", dataSet));

            apiList.ForEach((api) =>
            {
                Console.WriteLine("Testing {0} API.", api.ApiName);
                Task apiRun = api.RunApi();
                apiRun.Wait();
                Console.WriteLine("API {0} Complete. Exporting results...", api.ApiName);
                api.ExportResults("D:\\Api Output");
                Console.WriteLine("Export Complete.", api.ApiName);
            });

            Console.WriteLine("Press Any Key To Continue...");
            Console.ReadKey();
        }
    }
}
