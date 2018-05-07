using FaceTAN.Core.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unirest_net.http;

namespace FaceTAN.Core.ApiHandler
{
    public class SkyBiometryApiHandler : BaseApiHandler
    {
        public SkyBiometryApiHandler(string accessKey, string secretKey, string baseUrl, string ns, DataSet dataSet)
        {
            ApiName = "SkyBiometry";
            AccessKey = accessKey;
            SecretKey = secretKey;
            BaseUrl = baseUrl;
            NameSpace = ns;
            DataSet = dataSet;
        }
        
        private string AccessKey { get; set; }

        private string SecretKey { get; set; }

        private string BaseUrl { get; set; }

        private DataSet DataSet { get; set; }

        private string NameSpace { get; set; }

        private List<string> GroupFacesResults { get; set; }

        private List<string> RecognizeFacesResults { get; set; }

        public override async Task RunApi()
        {
            GroupFacesResults = GroupFaces();
            RecognizeFacesResults = RecognizeFaces();
        }

        private List<string> GroupFaces()
        {
            List<string> responses = new List<string>();
            var request = $"https://face.p.mashape.com/faces/group?api_key={AccessKey}&api_secret={SecretKey}";
            foreach (var entry in DataSet.TargetImages)
            {
                var responseJson = Unirest.post(request)
                    .header("X-Mashape-Key", "poBqadxqvtmsh41oAkQaextKC76rp1BI0gsjsnkufddJ8jP7PF")
                    .header("Content-Type", "application/x-www-form-urlencoded")
                    .header("Accept", "application/json")
                    .field("attributes", "all")
                    .field("detector", "Aggressive")
                    .field("urls", $"https://s3-ap-southeast-2.amazonaws.com/capstone-dataset/{entry.Key}")
                    .field("namespace", "MyNamespace")
                    .field("threshold", 30)
                    .field("uids", "all")
                    .asString().Body;
                responses.Add(responseJson);
            }
            return responses;
        }

        private List<string> RecognizeFaces()
        {
            List<string> responses = new List<string>();
            var request = $"https://face.p.mashape.com/faces/recognize?api_key={AccessKey}&api_secret={SecretKey}";
            foreach (var entry in DataSet.TargetImages)
            {
                var responseJson = Unirest.post(request)
                    .header("X-Mashape-Key", "poBqadxqvtmsh41oAkQaextKC76rp1BI0gsjsnkufddJ8jP7PF")
                    .header("Content-Type", "application/x-www-form-urlencoded")
                    .header("Accept", "application/json")
                    .field("attributes", "all")
                    .field("detector", "Aggressive")
                    .field("limit", 10)
                    .field("urls", $"https://s3-ap-southeast-2.amazonaws.com/capstone-dataset/{entry.Key}")
                    .field("namespace", "MyNamespace")
                    .field("uids", "all")
                    .asString().Body;
                responses.Add(responseJson);
            }
            return responses;
        }

        public override void ExportResults(string outputDirectory)
        {
            Directory.CreateDirectory(outputDirectory + "\\SkyBiometry");

            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter file = File.CreateText(outputDirectory + "\\SkyBiometry\\Group_Faces_Data.json"))
            {
                serializer.Serialize(file, GroupFacesResults);
                Console.WriteLine("Wrote azure target face data to {0}.", outputDirectory + "\\SkyBiometry\\Group_Faces_Data.json");
            }

            using (StreamWriter file = File.CreateText(outputDirectory + "\\SkyBiometry\\Recognize_Faces_Data.json"))
            {
                serializer.Serialize(file, RecognizeFacesResults);
                Console.WriteLine("Wrote azure target face data to {0}.", outputDirectory + "\\SkyBiometry\\Recognize_Faces_Data.json");
            }
        }
    }
}
