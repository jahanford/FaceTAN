using Amazon;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using FaceTAN.Core.ApiHandler;
using FaceTAN.Core.Data;
using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceTAN.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            // Setup DataSet
            DataSet dataSet = new DataSet("capstone-dataset", "AKIAJJKYA2TLOIPHNNVA", "BBN6C1W3Lx0bo+mOgmD7xjlfstoA3qKA8ppIr38A", 8);

            //Setup & Run Amazon Api
            Console.WriteLine("Setting up Amazon Rekognition API...");
            AmazonRekognitionConfig rekognitionConfig = new AmazonRekognitionConfig
            {
                RegionEndpoint = RegionEndpoint.USWest2
            };
            AmazonApiHandler rekognition = new AmazonApiHandler("AKIAJJKYA2TLOIPHNNVA", "BBN6C1W3Lx0bo+mOgmD7xjlfstoA3qKA8ppIr38A", rekognitionConfig, dataSet, "testcollection");
            //rekognition.RunApi();
            Console.WriteLine("Amazon Rekognition API run complete.");

            // Setup & Run Azure Api
            AzureApiHandler azure = new AzureApiHandler("3ab30cd064c04013bd868bf7d7c8a2f4", "https://westcentralus.api.cognitive.microsoft.com/face/v1.0", "", "test-person-group", dataSet);
            Task azureTask = azure.RunApi();
            azureTask.Wait();

            Console.WriteLine("Press Any Key To Continue...");
            Console.ReadKey();
        }
    }
}
