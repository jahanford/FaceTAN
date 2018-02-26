using Amazon;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using Amazon.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Drawing;

namespace FaceTAN.Core
{

    public static class RekognitionHelpers
    {

        public static CompareFacesRequest GenerateCompareFacesRequest(System.Drawing.Image sourceImage, System.Drawing.Image targetImage, int similarityThreshold = 90)
        {
            MemoryStream sourceImageStream = Helpers.ImageToMemoryStream(sourceImage);
            MemoryStream targetImageStream = Helpers.ImageToMemoryStream(targetImage);

            return new CompareFacesRequest
            {
                SimilarityThreshold = similarityThreshold,

                SourceImage = new Amazon.Rekognition.Model.Image
                {
                    Bytes = sourceImageStream
                },
                TargetImage = new Amazon.Rekognition.Model.Image
                {
                    Bytes = targetImageStream
                }
            };
        }

        public static DetectFacesRequest GenerateDetectFacesRequest(System.Drawing.Image inputImage, String attributes = "ALL")
        {
            MemoryStream inputImageStream = Helpers.ImageToMemoryStream(inputImage);

            return new DetectFacesRequest
            {
                Attributes = new List<String>(new string[] { attributes }),
                Image = new Amazon.Rekognition.Model.Image
                {
                    Bytes = inputImageStream
                }
            };
        }
    }

    public class AmazonRekognitionHandler : FacialRecognitionAPI<string, string, AmazonRekognitionConfig>
    {

        public override string APIName => "Amazon Rekognition";
        private AmazonRekognitionClient client;

        public AmazonRekognitionHandler(string accessKey, string secretKey, [Optional]AmazonRekognitionConfig rekognitionConfig) : base(accessKey, secretKey, rekognitionConfig)
        {
            //Handle AWS Credential Handshake
            AWSAuthentificationHandler AWS = new AWSAuthentificationHandler(accessKey, secretKey);

            //Construct the Rekognition Client
            if (rekognitionConfig == null)
            {
                rekognitionConfig = new AmazonRekognitionConfig
                {
                    RegionEndpoint = RegionEndpoint.USWest2
                };

            }

            client = new AmazonRekognitionClient(AWS.Credentials, rekognitionConfig);
        }

        public DetectFacesResponse DetectFaces(DetectFacesRequest detectrequestObject)
        {

            try
            {
                return AsyncHelpers.RunSync<DetectFacesResponse>(() => client.DetectFacesAsync(detectrequestObject));

            }
            catch (AmazonRekognitionException e)
            {
                Console.Write(e.Message);
                return null;
            }
        }

        public CompareFacesResponse CompareFaces(CompareFacesRequest compareObject)
        {
            try
            {
                return AsyncHelpers.RunSync<CompareFacesResponse>(() => client.CompareFacesAsync(compareObject));

            }
            catch (AmazonRekognitionException e)
            {
                Console.Write(e.Message);
                return null;
            }
        }

        //AWS Credential Authentification Handler
        private class AWSAuthentificationHandler
        {
            public AWSCredentials Credentials { get; }

            public AWSAuthentificationHandler(string accessKey, string secretKey)
            {
                try
                {
                    Credentials = new BasicAWSCredentials(accessKey, secretKey);
                }
                catch (Exception e)
                {
                    throw new AmazonClientException(
                        "Cannot load the credentials from the credential profiles file. " +
                        "Please make sure that your credentials file is at the correct " +
                        "location (/Users/userid/.aws/credentials), and is in a valid format.",
                        e);
                }
            }
        }
    }
}
