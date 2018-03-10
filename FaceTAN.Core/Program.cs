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
            DataSet dataSet = new DataSet("capstone-dataset", "AKIAJJKYA2TLOIPHNNVA", "BBN6C1W3Lx0bo+mOgmD7xjlfstoA3qKA8ppIr38A", 255);

            // Setup Amazon Api
            Console.WriteLine("Setting up Amazon Rekognition API...");
            AmazonRekognitionConfig rekognitionConfig = new AmazonRekognitionConfig
            {
                RegionEndpoint = RegionEndpoint.USWest2
            };
            AmazonApiHandler rekognition = new AmazonApiHandler("AKIAJJKYA2TLOIPHNNVA", "BBN6C1W3Lx0bo+mOgmD7xjlfstoA3qKA8ppIr38A", rekognitionConfig, dataSet);
            rekognition.RunApi(10);

            //AmazonRekognitionHandler rekognitionContext = new AmazonRekognitionHandler("AKIAI2T6XBLFQ5XTPDDA", "m8EYnJl21SB3JeXzHNDAdlLJt3v3is0jGajVdSDX");
            //AzureFaceApiHandler azureContext = new AzureFaceApiHandler("1e16475cb1374f37ae67e43509933e2d", "https://westcentralus.api.cognitive.microsoft.com/face/v1.0");

            /*
            var groupID = Guid.NewGuid();
            var James = azureContext.CreatePerson("James");
            if (azureContext.CreatePersonGroup(groupID.ToString(), "Test Group"))
            {
                var x = azureContext.GetPersonGroup(groupID.ToString());
                Console.WriteLine("Successfully created group!\nGroup Name: {0}\nGroup ID: {1}", x.Name, x.PersonGroupId);
            }*/

            /**************************
             TESTS
            **************************/
            //AZURE Face API Example Test
            /*TestAzure testInstance;
            testInstance = new TestAzure(azureContext, "TestImages/EXAMPLE", "exampleImage1.jpg", "exampleImage2.jpg", true);
            testInstance.Run();
            testInstance = null;

            Console.WriteLine("\n");

            //AMAZON Rekognition Example Test
            TestAmazon Test;
            Test = new TestAmazon(rekognitionContext, "TestImages/EXAMPLE", "exampleImage1.jpg", "exampleImage2.jpg", true);
            Test.Run();
            Test = null;*/

            //Prevent Console Exiting
            Console.WriteLine("Press Any Key To Continue...");
            Console.ReadKey();
        }
    }

    public class TestAzure
    {

        String imageOneName;
        String imageTwoName;
        String testFolderName;
        System.Drawing.Image imageOne;
        System.Drawing.Image imageTwo;
        Boolean expectMatch;
        AzureFaceApiHandler apiInstance;

        public TestAzure(AzureFaceApiHandler apiInstance, String testFolderName, String imageOneName, String imageTwoName, Boolean expectMatch)
        {
            this.apiInstance = apiInstance;
            this.imageOneName = imageOneName;
            this.imageTwoName = imageTwoName;
            this.testFolderName = testFolderName;
            imageOne = System.Drawing.Image.FromFile(Path.GetFullPath(@"..\..\" + @""+ testFolderName + "/" + imageOneName));
            imageTwo = System.Drawing.Image.FromFile(Path.GetFullPath(@"..\..\" + @"" + testFolderName + "/" + imageTwoName));


            this.expectMatch = expectMatch;
        }

        public void Run()
        {

            string imageOnePath = Path.GetFullPath(@"..\..\" + @"" + testFolderName + "/" + imageOneName);
            var detectOne = apiInstance.DetectFaces(imageOnePath);

            //Helpers.PrintFaceDetailList(x.FaceDetails);

            foreach (Microsoft.ProjectOxford.Face.Contract.Face face in detectOne)
            {
                System.Drawing.Image faceImage = Helpers.CropFaceToImage(imageOne, face);
                Console.WriteLine("Cropping: " + imageOnePath);
                faceImage.Save(imageOnePath.Insert(imageOnePath.Length - 4, "AZURE"));
            }

            string imageTwoPath = Path.GetFullPath(@"..\..\" + @"" + testFolderName + "/" + imageTwoName);
            var detectTwo = apiInstance.DetectFaces(imageTwoPath);
            foreach (Microsoft.ProjectOxford.Face.Contract.Face face in detectTwo)
            {
                System.Drawing.Image faceImage = Helpers.CropFaceToImage(imageTwo, face);
                Console.WriteLine("Cropping: " + imageTwoPath);
                faceImage.Save(imageTwoPath.Insert(imageTwoPath.Length - 4, "AZURE"));
            }

            //Compare
            if (detectOne.Length > 0 && detectTwo.Length > 0)
            {
                VerifyResult result = apiInstance.CompareFaces(detectOne[0].FaceId, detectTwo[0].FaceId);
                Console.WriteLine("Is Match: {0} Confidence: {1}", result.IsIdentical, result.Confidence);
            }

        }
    }

    public class TestAmazon
    {

        String imageOneName;
        String imageTwoName;
        String testFolderName;
        System.Drawing.Image imageOne;
        System.Drawing.Image imageTwo;
        Boolean expectMatch;
        AmazonRekognitionHandler rekognitionContext;

        public TestAmazon(AmazonRekognitionHandler rekognition, String testFolderName, String imageOneName, String imageTwoName, Boolean expectMatch)
        {
            rekognitionContext = rekognition;
            this.imageOneName = imageOneName;
            this.imageTwoName = imageTwoName;
            this.testFolderName = testFolderName;
            imageOne = System.Drawing.Image.FromFile(Path.GetFullPath(@"..\..\" + @"" + testFolderName + "/" + imageOneName));
            imageTwo = System.Drawing.Image.FromFile(Path.GetFullPath(@"..\..\" + @"" + testFolderName + "/" + imageTwoName));

            this.expectMatch = expectMatch;
        }

        public void Run(String title = "")
        {
            //Detect Faces In Image One
            Console.WriteLine("Detecting Image One Faces...");
            DetectFacesRequest detectOneRequest = RekognitionHelpers.GenerateDetectFacesRequest(imageOne);
            DetectFacesResponse detectedOne = rekognitionContext.DetectFaces(detectOneRequest);
            //Helpers.PrintFaceDetailList(detectedOne.FaceDetails, imageOneName + " -");
            Helpers.CropDetectedFaces(detectedOne.FaceDetails, Path.GetFullPath(@"..\..\" + @"" + testFolderName + "/" + imageOneName), true);

            //Detect Faces In Image Two
            Console.WriteLine("Detecting Image Two Faces...");
            DetectFacesRequest detectTwoRequest = RekognitionHelpers.GenerateDetectFacesRequest(imageTwo);
            DetectFacesResponse detectedTwo = rekognitionContext.DetectFaces(detectTwoRequest);
            //Helpers.PrintFaceDetailList(detectedTwo.FaceDetails, imageTwoName + " -");
            Helpers.CropDetectedFaces(detectedTwo.FaceDetails, Path.GetFullPath(@"..\..\" + @"" + testFolderName + "/" + imageTwoName), true);

            //Comparing Faces
            Console.WriteLine("Comparing Faces...");

            CompareFacesRequest request = RekognitionHelpers.GenerateCompareFacesRequest(imageOne, imageTwo);
            CompareFacesResponse result = rekognitionContext.CompareFaces(request);

            BoundingBox sourceImage = result.SourceImageFace.BoundingBox;
            Console.WriteLine("[Source Image Bounding Box] Left Ratio: " + sourceImage.Left + " Top Ratio: " + sourceImage.Top);


            if (!expectMatch)
            {
                //EXPECT NO MATCH
                if (result.FaceMatches.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("[TEST SUCCESS] No Match was found as expected.");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[TEST FAILURE] No match was expected but " + result.FaceMatches.Count.ToString() + " was found?");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }
            else
            {
                //EXPECT MATCH
                if (result.FaceMatches.Count > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("[TEST SUCCESS] A Match was found as expected. Count = " + result.FaceMatches.Count);
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[TEST FAILURE] No match was found but one was expected? Count = " + result.FaceMatches.Count);
                    Console.ForegroundColor = ConsoleColor.Gray;
                }

            }

            Console.WriteLine();
        }
    }
}
