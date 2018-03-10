using FaceTAN.Core.ApiRequest;
using FaceTAN.Core.ApiResponse;
using Amazon.Runtime;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using System.IO;
using System.Collections.Generic;

namespace FaceTAN.Core.ApiHandler
{
    public class AmazonApiHandler
    {
        public AmazonApiHandler(string accessKey, string secretKey, AmazonRekognitionConfig rekognitionConfig)
        {
            AccessKey = accessKey;
            SecretKey = secretKey;
            Credentials = new BasicAWSCredentials(AccessKey, SecretKey);
            Client = new AmazonRekognitionClient(Credentials, rekognitionConfig);
        }

        private AWSCredentials Credentials { get; }

        private AmazonRekognitionClient Client { get; }

        private string AccessKey { get; }

        private string SecretKey { get; }

        public AmazonApiResponse RunApi(AmazonApiRequest request)
        {
            throw new System.NotImplementedException();
        }

        private static DetectFacesRequest GenerateDetectRequest(System.Drawing.Image inputImage, string attributes = "ALL")
        {
            MemoryStream inputImageStream = Helpers.ImageToMemoryStream(inputImage);

            return new DetectFacesRequest
            {
                Attributes = new List<string>(new string[] { attributes }),
                Image = new Image
                {
                    Bytes = inputImageStream
                }
            };
        }
    }
}
