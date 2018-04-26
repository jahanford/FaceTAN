using Amazon.Runtime;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using FaceTAN.Core.Data.Models.Timing;
using System.IO;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using FaceTAN.Core.Data;
using System.Threading.Tasks;
using Amazon;
using Newtonsoft.Json;

namespace FaceTAN.Core.ApiHandler
{
    public class AmazonApiHandler : BaseApiHandler
    {
        public AmazonApiHandler(ApiKeyStore accessKeys, ApiKeyStore secretKeys, DataSet dataSet, string collectionName)
        {
            ApiName = "Amazon Rekognition";
            AccessKeys = accessKeys;
            SecretKeys = secretKeys;
            CollectionName = collectionName;
            Credentials = new BasicAWSCredentials(AccessKeys.GetCurrentKey(), SecretKeys.GetCurrentKey());
            Client = new AmazonRekognitionClient(Credentials, new AmazonRekognitionConfig { RegionEndpoint = RegionEndpoint.USWest2 });
            DataSet = dataSet;
            IndexedFaces = new List<IndexFacesResponse>();
            MatchResults = new List<SearchFacesByImageResponse>();
            TimingResults = new List<TimingModel>();
        }

        private AWSCredentials Credentials { get; }

        private AmazonRekognitionClient Client { get; }

        private DataSet DataSet { get; }

        private ApiKeyStore AccessKeys { get; }

        private ApiKeyStore SecretKeys { get; }

        private string CollectionName { get; }

        private List<IndexFacesResponse> IndexedFaces { get; set; }

        private List<SearchFacesByImageResponse> MatchResults { get; set; }

        private List<TimingModel> TimingResults { get; set; }

        public override async Task RunApi()
        {
            if (CollectionExists())
                DeleteCollection();

            CreateCollection();
            List<FaceRecord> faceRecords = PopulateCollection();
            List<FaceMatch> faceMatches = SearchCollectionForSourceImageFaces();
        }

        public override void ExportResults(string outputDirectory)
        {
            Directory.CreateDirectory(outputDirectory + "\\Rekognition");

            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter file = File.CreateText(outputDirectory + "\\Rekognition\\Rekognition_Indexed_Faces.txt"))
            {
                serializer.Serialize(file, IndexedFaces);
                Console.WriteLine("Wrote rekognition index face data to {0}.", outputDirectory + "\\Rekognition\\Rekognition_Indexed_Faces.txt");
            }
            using (StreamWriter file = File.CreateText(outputDirectory + "\\Rekognition\\Rekognition_Match_Results.txt"))
            {
                serializer.Serialize(file, MatchResults);
                Console.WriteLine("Wrote rekognition face match data to {0}.", outputDirectory + "\\Rekognition\\Rekognition_Match_Results.txt");
            }
            using (StreamWriter file = File.CreateText(outputDirectory + "\\Rekognition\\Rekognition_Timing_Results.txt"))
            {
                serializer.Serialize(file, TimingResults);
                Console.WriteLine("Wrote rekognition face match data to {0}.", outputDirectory + "\\Rekognition\\Rekognition_Timing_Results.txt");
            }
        }

        private bool CollectionExists()
        {
            ListCollectionsRequest request = new ListCollectionsRequest();
            ListCollectionsResponse response = Client.ListCollections(request);
            return response.CollectionIds.Contains(CollectionName);
        }

        private void DeleteCollection()
        {
            Console.WriteLine("Deleting existing collection: {0}", CollectionName);
            DeleteCollectionRequest request = new DeleteCollectionRequest()
            {
                CollectionId = CollectionName
            };

            DeleteCollectionResponse response = Client.DeleteCollection(request);
            Console.WriteLine("Collection deleted.");
        }
            
        private string CreateCollection()
        {
            Console.WriteLine("Creating rekognition collection: {0}", CollectionName);
            CreateCollectionRequest request = new CreateCollectionRequest()
            {
                CollectionId = CollectionName
            };

            CreateCollectionResponse response = Client.CreateCollection(request);
            Console.WriteLine("Collection created with ARN: {0}", response.CollectionArn);
            return response.CollectionArn;
        }

        private List<FaceRecord> PopulateCollection()
        {
            Console.WriteLine("Populating collection {0}: ", CollectionName);
            List<FaceRecord> result = new List<FaceRecord>();

            foreach(var entry in DataSet.TargetImages)
            {
                Console.WriteLine("Processing image: {0}", entry.Key);
                MemoryStream stream = new MemoryStream();
                entry.Value.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);

                Image requestImage = new Image()
                {
                    Bytes = stream
                };

                IndexFacesRequest request = new IndexFacesRequest()
                {
                    Image = requestImage,
                    CollectionId = CollectionName
                };

                var watch = Stopwatch.StartNew();
                IndexFacesResponse response = Client.IndexFaces(request);
                watch.Stop();
                TimingResults.Add(new TimingModel("PopulateCollection", entry.Key, watch.ElapsedMilliseconds));
                IndexedFaces.Add(response);
                result.AddRange(response.FaceRecords);
            }

            Console.WriteLine("Detected {0} faces in {1} images.", result.Count, DataSet.TargetImages.Count);
            return result;
        }

        private List<FaceMatch> SearchCollectionForSourceImageFaces()
        {
            List<FaceMatch> result = new List<FaceMatch>();
            Console.WriteLine("Searching target collection for matching source faces...");

            foreach(var entry in DataSet.SourceImages)
            {
                Console.WriteLine("Attempting to match image {0}.", entry.Key);
                MemoryStream stream = new MemoryStream();
                entry.Value.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);

                Image requestImage = new Image()
                {
                    Bytes = stream
                };

                SearchFacesByImageRequest request = new SearchFacesByImageRequest()
                {
                    Image = requestImage,
                    CollectionId = CollectionName
                };

                var watch = Stopwatch.StartNew();
                SearchFacesByImageResponse response = Client.SearchFacesByImage(request);
                watch.Stop();
                TimingResults.Add(new TimingModel("SearchCollectionForSourceImageFaces", entry.Key, watch.ElapsedMilliseconds));
                MatchResults.Add(response);

                if (response.FaceMatches.Count > 0)
                    Console.WriteLine("Matching target face found for {0} with a confidence level of {1}.", entry.Key, response.SearchedFaceConfidence);
                else
                    Console.WriteLine("No matching target face found for {0}.", entry.Key);

                result.AddRange(response.FaceMatches);
            }

            Console.WriteLine("{0} out of {1} faces successfully matched.", result.Count, DataSet.SourceImages.Count);
            return result;
        }
    }
}
