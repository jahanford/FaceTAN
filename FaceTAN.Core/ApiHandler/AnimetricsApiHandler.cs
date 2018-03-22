using FaceTAN.Core.Data;
using FaceTAN.Core.Data.Models.Animetrics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using unirest_net.http;

namespace FaceTAN.Core.ApiHandler
{
    public class AnimetricsApiHandler : BaseApiHandler
    {
        public AnimetricsApiHandler(string apiKey, string baseUrl, string galleryId, DataSet dataSet)
        {
            ApiName = "Animetrics FaceR";
            ApiKey = apiKey;
            BaseUrl = baseUrl;
            GalleryId = galleryId;
            DataSet = dataSet;
        }

        private string ApiKey { get; }

        private string BaseUrl { get; }

        private string GalleryId { get; }

        private DataSet DataSet { get; }

        private Dictionary<string, AnimetricsDetectResponse> DetectedTargetFaces { get; set; }

        private Dictionary<string, AnimetricsDetectResponse> DetectedSourceFaces { get; set; }

        private List<AnimetricsEnrollResponse> EnrolledFaces { get; set; }

        private List<AnimetricsRecognizeResponse> RecognizedFaces { get; set; }

        public override async Task RunApi()
        {
            Console.WriteLine("Detecting target faces...");
            DetectedTargetFaces = DetectFaces(DataSet.TargetImages);

            if (DetectedTargetFaces == null)
                return;

            Console.WriteLine("Detecting source faces...");
            DetectedSourceFaces = DetectFaces(DataSet.SourceImages);

            if (DetectedTargetFaces == null)
                return;

            Console.WriteLine("Enrolling target faces...");
            EnrolledFaces = EnrollTargetFaces();

            Console.WriteLine("Recognizing source faces...");
            RecognizedFaces = RecognizeSourceFaces();
        }

        public override void ExportResults(string outputDirectory)
        {
            Directory.CreateDirectory(outputDirectory + "\\Animetrics");

            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter file = File.CreateText(outputDirectory + "\\Animetrics\\Animetrics_Detected_Target_Faces.txt"))
            {
                serializer.Serialize(file, DetectedTargetFaces);
                Console.WriteLine("Wrote animetrics detected target face data to {0}.", outputDirectory + "\\Animetrics\\Animetrics_Detected_Target_Faces.txt");
            }
            using (StreamWriter file = File.CreateText(outputDirectory + "\\Animetrics\\Animetrics_Detected_Source_Faces.txt"))
            {
                serializer.Serialize(file, DetectedSourceFaces);
                Console.WriteLine("Wrote animetrics detected source face data to {0}.", outputDirectory + "\\Animetrics\\Animetrics_Detected_Source_Faces.txt");
            }
            using (StreamWriter file = File.CreateText(outputDirectory + "\\Animetrics\\Animetrics_Enrolled_Faces.txt"))
            {
                serializer.Serialize(file, EnrolledFaces);
                Console.WriteLine("Wrote animetrics enrolled face data to {0}.", outputDirectory + "\\Animetrics\\Animetrics_Enrolled_Faces.txt");
            }
            using (StreamWriter file = File.CreateText(outputDirectory + "\\Animetrics\\Animetrics_Recognized_Faces.txt"))
            {
                serializer.Serialize(file, EnrolledFaces);
                Console.WriteLine("Wrote animetrics recognized face data to {0}.", outputDirectory + "\\Animetrics\\Animetrics_Recognized_Faces.txt");
            }
        }

        private Dictionary<string, AnimetricsDetectResponse> DetectFaces(Dictionary<string, System.Drawing.Image> images)
        {
            Dictionary<string, AnimetricsDetectResponse> result = new Dictionary<string, AnimetricsDetectResponse>();
            foreach (var entry in images)
            {
                MemoryStream imageStream = new MemoryStream();
                DataSet.GetImage(entry.Key).Save(imageStream, System.Drawing.Imaging.ImageFormat.Jpeg);

                string responseJson = Unirest.post(BaseUrl + "detect")
                    .header("Accept", "application/json")
                    .field("api_key", ApiKey)
                    .field("selector", "FULL")
                    .field("image", imageStream.ToArray())
                    .asString().Body;

                AnimetricsDetectResponse response = JsonConvert.DeserializeObject<AnimetricsDetectResponse>(responseJson);

                if (response.images == null)
                {
                    Console.WriteLine("Error: {0}.", responseJson);
                    return null;
                }

                if (response.images[0].faces.Count > 0 && response != null)
                {
                    Console.WriteLine("Face found in image {0}.", entry.Key);
                    result.Add(entry.Key, response);
                }
                else
                {
                    Console.WriteLine("No faces found in image {0}.", entry.Key);
                }
            }

            return result;
        }

        private List<AnimetricsEnrollResponse> EnrollTargetFaces()
        {
            List<AnimetricsEnrollResponse> result = new List<AnimetricsEnrollResponse>();

            foreach (var entry in DetectedTargetFaces)
            {
                HttpResponse<string> response = Unirest.post(BaseUrl + "detect")
                    .header("Accept", "application/json")
                    .field("api_key", ApiKey)
                    .field("subject_id", entry.Key)
                    .field("gallery_id", GalleryId)
                    .field("image_id", entry.Value.images[0].image_id)
                    .field("topLeftX", entry.Value.images[0].faces[0].topLeftX)
                    .field("topLeftY", entry.Value.images[0].faces[0].topLeftY)
                    .field("width", entry.Value.images[0].width)
                    .field("height", entry.Value.images[0].height)
                    .asString();
                Console.WriteLine("Image {0} enrolled.", entry.Key);
                result.Add(JsonConvert.DeserializeObject<AnimetricsEnrollResponse>(response.Body));
            }
            return result;
        }

        private List<AnimetricsRecognizeResponse> RecognizeSourceFaces()
        {
            List<AnimetricsRecognizeResponse> result = new List<AnimetricsRecognizeResponse>();

            foreach (var entry in DetectedSourceFaces)
            {
                HttpResponse<string> response = Unirest.post(BaseUrl + "detect")
                    .header("Accept", "application/json")
                    .field("api_key", ApiKey)
                    .field("gallery_id", GalleryId)
                    .field("image_id", entry.Value.images[0].image_id)
                    .field("max_num_results", 10)
                    .field("topLeftX", entry.Value.images[0].faces[0].topLeftX)
                    .field("topLeftY", entry.Value.images[0].faces[0].topLeftY)
                    .field("width", entry.Value.images[0].width)
                    .field("height", entry.Value.images[0].height)
                    .asString();
                Console.WriteLine("Face recognized in source image {0}.", entry.Key);
                result.Add(JsonConvert.DeserializeObject<AnimetricsRecognizeResponse>(response.Body));
            }
            return result;
        }
    }
}
