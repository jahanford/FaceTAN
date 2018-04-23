using FaceTAN.Core.Data;
using FaceTAN.Core.Data.Models.Lambda;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using unirest_net.http;

namespace FaceTAN.Core.ApiHandler
{
    public class LambdaApiHandler : BaseApiHandler
    {
        public LambdaApiHandler(ApiKeyStore apiKeys, string baseUrl, DataSet dataSet)
        {
            ApiName = "Lambda Labs";
            ApiKeys = apiKeys;
            BaseUrl = baseUrl;
            DataSet = dataSet;
        }

        private ApiKeyStore ApiKeys { get; set; }

        private string AlbumName { get; set; }

        private string BaseUrl { get; set; }

        private DataSet DataSet { get; set; }

        private LambdaCreateAlbumResponse AlbumResponse { get; set; }

        private List<LambdaTrainAlbumResponse> TrainResponses { get; set; }

        private LambdaRebuildAlbumResponse RebuildResponse { get; set; }

        private List<LambdaRecognizeResponse> RecognizeResponses { get; set; }

        public override async Task RunApi()
        {
            AlbumResponse = CreateAlbum();

            AlbumName = AlbumResponse.album;

            TrainResponses = TrainAlbum(AlbumResponse.albumkey);
            RebuildResponse = RebuildAlbum(AlbumResponse.albumkey);
            RecognizeResponses = RecognizePhotos(AlbumResponse.albumkey);
        }

        public override void ExportResults(string outputDirectory)
        {
            Directory.CreateDirectory(outputDirectory + "\\Lambda");

            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter file = File.CreateText(outputDirectory + "\\Lambda\\Lambda_Create_Album_Response.txt"))
            {
                serializer.Serialize(file, AlbumResponse);
                Console.WriteLine("Wrote lambda create album response data to {0}.", outputDirectory + "\\Lambda\\Lambda_Create_Album_Response.txt");
            }
            using (StreamWriter file = File.CreateText(outputDirectory + "\\Lambda\\Lambda_Train_Album.txt"))
            {
                serializer.Serialize(file, TrainResponses);
                Console.WriteLine("Wrote lambda train album data to {0}.", outputDirectory + "\\Lambda\\Lambda_Train_Album.txt");
            }
            using (StreamWriter file = File.CreateText(outputDirectory + "\\Lambda\\Lambda_Recognize_Photos.txt"))
            {
                serializer.Serialize(file, RecognizeResponses);
                Console.WriteLine("Wrote lambda recognize data to {0}.", outputDirectory + "\\Lambda\\Lambda_Recognize_Photos.txt");
            }
        }

        private LambdaCreateAlbumResponse CreateAlbum()
        {
            string responseJson = Unirest.post(BaseUrl + "album")
                .header("X-Mashape-Key", ApiKeys.GetCurrentKey())
                .header("Accept", "application/json")
                .field("album", Guid.NewGuid())
                .asString().Body;
            return JsonConvert.DeserializeObject<LambdaCreateAlbumResponse>(responseJson);
        }

        private List<LambdaTrainAlbumResponse> TrainAlbum(string albumId)
        {
            List<LambdaTrainAlbumResponse> result = new List<LambdaTrainAlbumResponse>();

            foreach(var entry in DataSet.TargetImages)
            {
                MemoryStream imageStream = new MemoryStream();
                DataSet.GetImage(entry.Key).Save(imageStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] data = imageStream.ToArray();

                // For some reason LambdaLabs is really picky about what characters they allow in an entry id
                string entryId = entry.Key.Split('/')[1].Split('.')[0].Replace("_", "");

                HttpResponse<string> response = Unirest.post(BaseUrl + "album_train")
                    .header("X-Mashape-Key", ApiKeys.GetCurrentKey())
                    .header("accept", "application/json")
                    .field("album", AlbumName)
                    .field("albumkey", albumId)
                    .field("entryid", entryId)
                    .field("files", data)
                    .asString();

                LambdaTrainAlbumResponse responseObject = JsonConvert.DeserializeObject<LambdaTrainAlbumResponse>(response.Body);

                if (responseObject.entryid != null)
                {
                    Console.WriteLine("Face detected in image {0}.", entry.Key);
                    result.Add(responseObject);
                }
                else
                {
                    Console.WriteLine("No face detected in image {0}.", entry.Key);
                }
            }

            return result;
        }

        private LambdaRebuildAlbumResponse RebuildAlbum(string albumId)
        {
            string responseJson = Unirest.get(BaseUrl + "album_rebuild" + string.Format("?album={0}&albumkey={1}", AlbumName, albumId))
                    .header("X-Mashape-Key", ApiKeys.GetCurrentKey())
                    .header("Accept", "application/json")
                    .asString().Body;
            return JsonConvert.DeserializeObject<LambdaRebuildAlbumResponse>(responseJson);
        }

        private List<LambdaRecognizeResponse> RecognizePhotos(string albumId)
        {
            List<LambdaRecognizeResponse> result = new List<LambdaRecognizeResponse>();

            foreach (var entry in DataSet.SourceImages)
            {
                MemoryStream imageStream = new MemoryStream();
                DataSet.GetImage(entry.Key).Save(imageStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] data = imageStream.ToArray();

                string responseJson = Unirest.post(BaseUrl + "recognize")
                    .header("X-Mashape-Key", ApiKeys.GetCurrentKey())
                    .header("Accept", "application/json")
                    .field("album", AlbumName)
                    .field("albumkey", albumId)
                    .field("files", data)
                    .asString().Body;

                LambdaRecognizeResponse response = JsonConvert.DeserializeObject<LambdaRecognizeResponse>(responseJson);

                if (response.status != "success")
                {
                    Console.WriteLine("API failed at image {0}.", entry.Key);
                }
                else
                {
                    result.Add(response);
                    if (response.photos[0].tags[0].uids == null)
                    {
                        Console.WriteLine("Image {0} not recognized.", entry.Key);
                    }
                    else
                    {
                        string prediction = response.photos[0].tags[0].uids[0].prediction;
                        double confidence = response.photos[0].tags[0].uids[0].confidence;
                        Console.WriteLine("Image {0} recognized as {1} with {2} confidence.", entry.Key, prediction, confidence);
                    }
                }
            }

            return result;
        }
    }
}
