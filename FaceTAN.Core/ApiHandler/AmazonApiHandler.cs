using FaceTAN.Core.ApiRequest;
using Amazon.Runtime;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using System.IO;
using System.Collections.Generic;
using System;
using FaceTAN.Core.Data;

namespace FaceTAN.Core.ApiHandler
{
    public class AmazonApiHandler
    {
        public AmazonApiHandler(string accessKey, string secretKey, AmazonRekognitionConfig rekognitionConfig, DataSet dataSet)
        {
            AccessKey = accessKey;
            SecretKey = secretKey;
            Credentials = new BasicAWSCredentials(AccessKey, SecretKey);
            Client = new AmazonRekognitionClient(Credentials, rekognitionConfig);
            DataSet = dataSet;
        }

        private AWSCredentials Credentials { get; }

        private AmazonRekognitionClient Client { get; }

        private DataSet DataSet { get; }

        private string AccessKey { get; }

        private string SecretKey { get; }

        public void RunApi(int maxImages)
        {
            string collectionName = "testcollection";

            if (CollectionExists(collectionName))
                DeleteCollection(collectionName);

            CreateCollection(collectionName);
            List<FaceRecord> faceRecords = PopulateCollection(collectionName, maxImages);

            Console.WriteLine("Detected {0} faces in {1} images.", faceRecords.Count, maxImages);
        }

        private bool CollectionExists(string collectionId)
        {
            ListCollectionsRequest request = new ListCollectionsRequest();
            ListCollectionsResponse response = Client.ListCollections(request);
            return response.CollectionIds.Contains(collectionId);
        }

        private void DeleteCollection(string collectionId)
        {
            Console.WriteLine("Deleting existing collection: {0}", collectionId);
            DeleteCollectionRequest request = new DeleteCollectionRequest()
            {
                CollectionId = collectionId
            };

            DeleteCollectionResponse response = Client.DeleteCollection(request);
            Console.WriteLine("Collection deleted.");
        }
            
        private string CreateCollection(string collectionId)
        {
            Console.WriteLine("Creating rekognition collection: {0}", collectionId);
            CreateCollectionRequest request = new CreateCollectionRequest()
            {
                CollectionId = collectionId
            };

            CreateCollectionResponse response = Client.CreateCollection(request);
            Console.WriteLine("Collection created with ARN: {0}", response.CollectionArn);
            return response.CollectionArn;
        }

        private List<FaceRecord> PopulateCollection(string collectionId, int maxImages)
        {
            Console.WriteLine("Populating collection {0}: ", collectionId);
            List<FaceRecord> result = new List<FaceRecord>();

            int count = 0;
            foreach (string key in DataSet.KeyList)
            {
                Console.WriteLine("Processing image: {0}", key);
                MemoryStream stream = new MemoryStream();
                DataSet.GetImage(key).Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);

                Image requestImage = new Image()
                {
                    Bytes = stream
                };

                IndexFacesRequest request = new IndexFacesRequest()
                {
                    Image = requestImage,
                    CollectionId = collectionId
                };

                IndexFacesResponse response = Client.IndexFaces(request);
                result.AddRange(response.FaceRecords);

                count += 1;
                if (count >= maxImages)
                    break;
            }

            return result;
        }
    }
}
