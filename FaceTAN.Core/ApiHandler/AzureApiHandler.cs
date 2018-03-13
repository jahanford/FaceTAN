using FaceTAN.Core.Data;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace FaceTAN.Core.ApiHandler
{
    public class AzureApiHandler : BaseApiHandler
    {
        public AzureApiHandler(string subscriptionKey, string region, string config, string personGroupId, DataSet dataSet)
        {
            ApiName = "Azure";
            Client = new FaceServiceClient(subscriptionKey, region);
            DataSet = dataSet;
            PersonGroupId = personGroupId;
            TargetFaceList = new List<Face>();
            SourceFaceList = new List<Face>();
            SourceMatchList = new List<IdentifyResult>();
        }        

        private FaceServiceClient Client { get; }

        private DataSet DataSet { get; }

        private string PersonGroupId { get; }

        private List<Face> TargetFaceList { get; set; }

        private List<Face> SourceFaceList { get; set; }

        private List<IdentifyResult> SourceMatchList { get; set; }


        public override async Task RunApi()
        {
            await InitApiAsync();
            await MatchSourceFaces();
            return;
        }

        private async Task InitApiAsync()
        {
            PersonGroup[] existingGroups = await Client.ListPersonGroupsAsync();
            if (existingGroups.ToList().Find(p => p.PersonGroupId == PersonGroupId) != null)
            {
                Console.WriteLine("Existing person group found with id {0}. Deleting...", PersonGroupId);
                await Client.DeletePersonGroupAsync(PersonGroupId);
            }

            Console.WriteLine("Creating person group: {0}.", PersonGroupId);
            await Client.CreatePersonGroupAsync(PersonGroupId, "Test Group");

            foreach (var entry in DataSet.TargetImages)
            {
                await AddTarget(entry);
            }
        }

        private async Task AddTarget(KeyValuePair<string, Image> entry)
        {
            Guid personId = new Guid();
            Face personFace = null;

            try
            {
                personId = await AddPerson(entry.Key);
            }
            catch (FaceAPIException e)
            {
                if (e.ErrorCode == "RateLimitExceeded")
                {
                    Console.WriteLine("API rate limit exceeded. Retrying in 60 seconds...");
                    Task.Delay(60000).Wait();
                }
                personId = await AddPerson(entry.Key);
            }

            if (personId == new Guid())
            {
                Console.WriteLine("Failed to create person {0}.", entry.Key);
                return;
            }

            try
            {
                personFace = await FindFace(entry.Key);
            }
            catch (FaceAPIException e)
            {
                if (e.ErrorCode == "RateLimitExceeded")
                {
                    Console.WriteLine("API rate limit exceeded. Retrying in 60 seconds...");
                    Task.Delay(60000).Wait();
                }
                await FindFace(entry.Key);
            }

            if (personFace == null)
            {
                Console.WriteLine("Failed to find face in image.");
                return;
            }

            try
            {
                await AddFaceToPerson(entry.Key, personId, personFace);
            }
            catch (FaceAPIException e)
            {
                if (e.ErrorCode == "RateLimitExceeded")
                {
                    Console.WriteLine("API rate limit exceeded. Retrying in 60 seconds...");
                    Task.Delay(60000).Wait();
                }
                await AddFaceToPerson(entry.Key, personId, personFace);
            }

            try
            {
                await TrainPersonGroup();
            }
            catch (FaceAPIException e)
            {
                if (e.ErrorCode == "RateLimitExceeded")
                {
                    Console.WriteLine("API rate limit exceeded. Retrying in 60 seconds...");
                    Task.Delay(60000).Wait();
                }
                await TrainPersonGroup();
            }
        }

        private async Task<Guid> AddPerson(string key)
        {
            Console.WriteLine("Attempting to creating person: {0}.", key);
            CreatePersonResult person = await Client.CreatePersonAsync(PersonGroupId, key);
            return person.PersonId;
        }

        private async Task<Face> FindFace(string key)
        {
            Console.WriteLine("Attempting to locate face in image.");
            Face[] faces = await Client.DetectAsync(DataSet.GetImageStream(key));


            if (faces.Length == 0)
                return null;
            else
            {
                TargetFaceList.AddRange(faces);
                return faces[0];
            }
        }

        private async Task AddFaceToPerson(string key, Guid personId, Face personFace)
        {
            Console.WriteLine("Adding face to person {0}.", key);
            await Client.AddPersonFaceAsync(PersonGroupId, personId, DataSet.GetImageStream(key), null, personFace.FaceRectangle);
        }

        private async Task TrainPersonGroup()
        {
            Console.WriteLine("Training PersonGroup {0} after adding new face.", PersonGroupId);
            await Client.TrainPersonGroupAsync(PersonGroupId);
        }

        private async Task MatchSourceFaces()
        {
            foreach(var entry in DataSet.SourceImages)
            {
                Console.WriteLine("Attempting to match face of person {0}.", entry.Key);

                Face[] faces = await Client.DetectAsync(DataSet.GetImageStream(entry.Key));
                Guid[] faceIds = faces.Select(face => face.FaceId).ToArray();
                IdentifyResult[] results = await Client.IdentifyAsync(PersonGroupId, faceIds);

                SourceFaceList.AddRange(faces);
                SourceMatchList.AddRange(results);

                foreach (var identifyResult in results)
                {
                    if (identifyResult.Candidates.Length == 0)
                        Console.WriteLine("Unable to find match.");
                    else
                    {
                        var candidateId = identifyResult.Candidates[0].PersonId;
                        Console.WriteLine("Face identified as {0}", entry.Key);
                    }
                }
            }
        }

        public override void ExportResults(string outputDirectory)
        {
            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter file = File.CreateText(outputDirectory + "\\Azure\\Azure_Target_Face_Data.txt"))
            {
                serializer.Serialize(file, TargetFaceList);
                Console.WriteLine("Wrote azure target face data to {0}.", outputDirectory + "\\Azure\\Azure_Target_Face_Data.txt");
            }
            using (StreamWriter file = File.CreateText(outputDirectory + "\\Azure\\Azure_Source_Face_Data.txt"))
            {
                serializer.Serialize(file, SourceFaceList);
                Console.WriteLine("Wrote azure source face data to {0}.", outputDirectory + "\\Azure\\Azure_Source_Face_Data.txt");
            }
            using (StreamWriter file = File.CreateText(outputDirectory + "\\Azure\\Azure_Match_Data.txt"))
            {
                serializer.Serialize(file, SourceMatchList);
                Console.WriteLine("Wrote azure face match data to {0}.", outputDirectory + "\\Azure\\Azure_Match_Data.txt");
            }
        }
    }
}
